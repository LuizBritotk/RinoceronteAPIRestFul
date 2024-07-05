using Rino.Dominio.DTOs.Usuario;
using Rino.Dominio.Util;

namespace Rino.Dominio.Interfaces.Negocio
{
    public interface IUsuarioNegocio
    {
        Task<RespostaPadrao> AutenticarUsuario(UsuarioLoginDTO usuarioLogin);
        Task<RespostaPadrao> Criar(UsuarioCriacaoDTO usuarioDTO);
        Task<RespostaPadrao> BuscarPorId(int id);
        Task<RespostaPadrao> BuscarTodos();
        Task<RespostaPadrao> Atualizar(int id, UsuarioAtualizarDTO usuarioDTO);
        Task<RespostaPadrao> Deletar(int id);
    }
}
