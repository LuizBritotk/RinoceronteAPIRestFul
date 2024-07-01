using Rino.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Dominio.Interfaces.Negocio
{
    public interface ICategoriaNegocio
    {
        Task<Categoria> Cadastrar(string codigoCategoria);
    }
}
