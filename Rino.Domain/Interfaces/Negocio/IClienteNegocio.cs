using Rino.Dominio.Entidades;
using Rino.Dominio.Util;
using System;
using System.Threading.Tasks;

namespace Rino.Dominio.Interfaces.Negocio
{
    public interface IClienteNegocio
    {
        Task<Cliente> CadastrarCliente(string codigoCliente);
    }
}
