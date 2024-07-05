using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rino.Dominio.Interfaces.Negocio;
using Rino.Dominio.Interfaces.Servicos;
using Rino.Dominio.Negocio;
using Rino.Dominio.Negocio.Servicos;
using Rino.Dominio.Validacoes;
using Rino.Infra.Configuracao;
using Rino.Infra.Servicos;
using Rino.Infra.Servicos.Autenticacao;
using Rino.Infra.ServicosExternos.Firebase;
using Rino.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Configuração de Serviços

// Adicionar serviços ao contêiner.
builder.Services.AddControllers();

// Configurar as configurações do JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configurar a autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

// Registrar ValidacoesCelulas
builder.Services.AddScoped<ValidacoesCelulasExcel>();
builder.Services.AddScoped<ValidacoesCelulasCSV>();

// Registrar Geradores de Dados Aleatórios
builder.Services.AddTransient<GeradorClienteAleatorio>();
builder.Services.AddTransient<GeradorProdutoAleatorio>();
builder.Services.AddTransient<GeradorCategoriaAleatorio>();

// Registrar serviços de negócio
builder.Services.AddTransient<IJwtServico, JwtServico>();
builder.Services.AddTransient<IFirebaseUsuarioServico, FirebaseUsuarioServico>();
builder.Services.AddTransient<IFirebaseArquivoServico, FirebaseArquivosServico>();
builder.Services.AddTransient<IUsuarioNegocio, UsuarioNegocio>();
builder.Services.AddTransient<IArquivosNegocio, ArquivosNegocio>();
builder.Services.AddTransient<ICategoriaNegocio, CategoriaNegocio>();
builder.Services.AddTransient<IClienteNegocio, ClienteNegocio>();
builder.Services.AddTransient<IProdutoNegocio, ProdutoNegocio>();
builder.Services.AddScoped<FirebaseUsuarioServico>();
builder.Services.AddScoped<FireBaseStore>();

// Configuração do limite de tamanho máximo de corpo de requisição (100 MB)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

// Configuração de Rate Limiting
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins("https://origemconfiavel.com")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

// Adicionar Swagger para documentação da API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rinoceronte API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira o JWT com Bearer no campo",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

#endregion

var app = builder.Build();

#region Configuração de Middleware

app.UseIpRateLimiting();

// Configurar o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rinoceronte API v1"));
}

app.UseHttpsRedirection();

// Middleware para configurar o limite de tamanho máximo de corpo de requisição (100 MB)
app.Use((context, next) =>
{
    context.Request.EnableBuffering();                                                     // Permite leitura repetida do corpo de requisição
    context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 104857600; // 100 MB

    return next();
});

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
