using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rino.Dominio.Interfaces.Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Rino.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArquivosController : ControllerBase
    {
        private readonly IArquivosNegocio _arquivosNegocio;

        private readonly Dictionary<string, Func<Stream, Task>> ExtensaoMetodoMap;

        public ArquivosController(IArquivosNegocio arquivoNegocio)
        {
            _arquivosNegocio = arquivoNegocio;
        }


        [HttpPost("upload")]
        [AllowAnonymous]

        //[Authorize(Roles = "Acesso Global")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadArquivo(IFormFile arquivo)
        {
            try
            {
                if (arquivo is null || arquivo.Length == 0)
                    return BadRequest("Nenhum arquivo enviado.");

                string extensao = Path.GetExtension(arquivo.FileName)?.ToLower()!;
                if (string.IsNullOrEmpty(extensao))
                    return BadRequest("Formato de arquivo inválido.");

                using (var stream = arquivo.OpenReadStream())
                {
                    var resposta = await _arquivosNegocio.ProcessarArquivo(stream, arquivo.FileName);
                    if (resposta.Error)
                        return StatusCode(500, resposta.Mensagem);
                }

                return Ok("Upload e processamento do arquivo concluídos com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno no servidor: {ex.Message}");
            }
        }
    }
}
