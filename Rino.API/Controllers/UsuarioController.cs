using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rino.Dominio.DTOs.Usuario;
using Rino.Dominio.Interfaces.Negocio;

namespace Rino.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioNegocio _usuarioNegocio;

        public UsuarioController(IUsuarioNegocio usuarioNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
        }

        // GET: v1/Usuario
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var usuarios = await _usuarioNegocio.BuscarTodos();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = $"Erro ao obter usuários: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var usuario = await _usuarioNegocio.BuscarPorId(id);

                if (usuario == null)
                    return NotFound();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = $"Erro ao obter usuário: {ex.Message}" });
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Criar([FromBody] UsuarioCriacaoDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var novoUsuario = await _usuarioNegocio.Criar(usuarioDTO);
                return CreatedAtAction(nameof(Criar), new { id = 123123 }, novoUsuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = $"Erro ao criar usuário: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioAtualizarDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuarioExistente = await _usuarioNegocio.BuscarPorId(id);

                if (usuarioExistente == null)
                    return NotFound();

                await _usuarioNegocio.Atualizar(id, usuarioDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = $"Erro ao atualizar usuário: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            try
            {
                var usuarioExistente = await _usuarioNegocio.BuscarPorId(id);

                if (usuarioExistente == null)
                    return NotFound();

                await _usuarioNegocio.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = $"Erro ao deletar usuário: {ex.Message}" });
            }
        }

    }
}
