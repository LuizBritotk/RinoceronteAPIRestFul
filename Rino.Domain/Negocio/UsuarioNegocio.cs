using Rino.Dominio.DTOs.Usuario;
using Rino.Dominio.Interfaces.Negocio;
using Rino.Dominio.Interfaces.Servicos;
using Rino.Dominio.Util;
using Rino.Infra.Servicos.Autenticacao;

namespace Rino.Dominio.Negocio
{
    public class UsuarioNegocio : IUsuarioNegocio
    {
        private readonly IFirebaseUsuarioServico _firebaseUsuarioServico;
        private readonly IJwtServico _jwtServico;

        public UsuarioNegocio(IFirebaseUsuarioServico firebaseUsuarioServico, IJwtServico jwtServico)
        {
            _firebaseUsuarioServico = firebaseUsuarioServico;
            _jwtServico = jwtServico;
        }

        public async Task<RespostaPadrao> AutenticarUsuario(UsuarioLoginDTO credenciais)
        {
            try
            {
                credenciais.Validate();

                if (!credenciais.IsValid)
                    return new RespostaPadrao(string.Join(" ", credenciais.Notifications), true, 400); // HTTP 400 Bad Request

                // Verifica se o login existe no Firebase Authentication
                var usuario = await _firebaseUsuarioServico.BuscarPorLogin(credenciais.Login);

                if (usuario is null)
                    return new RespostaPadrao("Usuário não encontrado.", true, 404); // HTTP 404 Not Found

                // Verifica se a senha é válida
                var validPassword = credenciais.Password == usuario.SenhaHash;

                if (!validPassword)
                    return new RespostaPadrao("Senha inválida.", true, 401); // HTTP 401 Unauthorized

                // Gera o JWT token para o usuário autenticado
                var token = _jwtServico.GenerateJwtToken(usuario.Id, usuario.Email);

                // Busca as claims do usuário no Firebase Database ou Firestore
                var claims = await _firebaseUsuarioServico.BuscarClaimsUsuario(usuario.Id);

                // Cria um DTO com os dados do usuário, suas claims e o token JWT
                var usuarioDTO = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Claims = claims,
                    Token = token
                };


                return new RespostaPadrao("Usuário autenticado com sucesso.", false, 200, usuarioDTO);
            }
            catch (Exception ex)
            {
                return new RespostaPadrao($"Erro ao autenticar usuário: {ex.Message}", true, 500);
            }
        }
    }
}
