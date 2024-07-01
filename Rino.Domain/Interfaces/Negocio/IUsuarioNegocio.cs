using Rino.Dominio.Util;

namespace Rino.Dominio.Interfaces.Negocio
{
    public interface IUsuarioNegocio
    {
        Task<RespostaPadrao> AutenticarUsuario(UsuarioLoginDTO usuarioLogin);
    }
}
