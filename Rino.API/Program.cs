using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rino.Dominio.Interfaces.Servicos;
using Rino.Infra.Servicos;
using Rino.Infra.ServicosExternos.Firebase;
using System.Text;
using Rino.Infra.Configuracao;
using Rino.Infra.Servicos.Autenticacao;
using Rino.Infrastructure.Data;
using Rino.Dominio.Negocio;
using Rino.Dominio.Interfaces.Negocio;
using Rino.Dominio.Validacoes;
using Rino.Dominio.Negocio.Servicos;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

#region Configurar Servi�os

// Adicionar servi�os ao cont�iner.
builder.Services.AddControllers();

// Configurar as configura��es do JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configurar a autentica��o JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

// Registrar GeradorClienteAleatorio e GeradorProdutoAleatorio, GeradorCategoriaAleatorio 
builder.Services.AddTransient<GeradorClienteAleatorio>();
builder.Services.AddTransient<GeradorProdutoAleatorio>();
builder.Services.AddTransient<GeradorCategoriaAleatorio>();

// Registrar servi�os
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

// Configura��o do limite de tamanho m�ximo de corpo de requisi��o (100 MB)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});


// Adicionar Swagger para documenta��o da API
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
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
    }});
});

#endregion

var app = builder.Build();

#region Configurar Middleware

// Configurar o pipeline de requisi��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rinoceronte API v1"));
}

app.UseHttpsRedirection();

// Middleware para configurar o limite de tamanho m�ximo de corpo de requisi��o (100 MB)
app.Use((context, next) =>
{
    context.Request.EnableBuffering(); // Permite leitura repetida do corpo de requisi��o
    context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 104857600; // 100 MB

    return next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
