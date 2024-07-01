using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Rino.Dominio.DTOs.Usuario;
using Rino.Dominio.Interfaces.Negocio;

namespace Rino.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IUsuarioNegocio _usuarioServico;

        public AutenticacaoController(IUsuarioNegocio usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO credenciais)
        {
            var usuarioAutenticado = await _usuarioServico.AutenticarUsuario(credenciais);

            if (usuarioAutenticado is null)
                return Unauthorized(new { mensagem = "Credenciais inválidas." });

            return Ok(usuarioAutenticado);
        }

        //[HttpPost("registrar")]
        //public async Task<IActionResult> Registrar([FromBody] UsuarioCriarDTO usuarioCadastro)
        //{
        //    var usuarioRegistrado = await _usuarioServico.RegistrarUsuario(usuarioCadastro);

        //    return CreatedAtAction(nameof(Login), new { id = usuarioRegistrado.Id }, usuarioRegistrado);
        //}
    }
}
