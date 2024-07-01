using Rino.Dominio.DTOs.Usuario;
using Rino.Dominio.Entidades;
using System.Threading.Tasks;

namespace Rino.Dominio.Interfaces.Servicos
{
    public interface IFirebaseUsuarioServico
    {
        Task<List<string>> BuscarClaimsUsuario(string usuarioId);
        Task<Usuario> BuscarPorLogin(string login);
    }
}
