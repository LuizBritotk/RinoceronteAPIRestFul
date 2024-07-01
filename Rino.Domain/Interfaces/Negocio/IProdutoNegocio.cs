using Rino.Dominio.DTOs.Produto;
using Rino.Dominio.Entidades;
using Rino.Dominio.Util;
using System;
using System.Threading.Tasks;

namespace Rino.Dominio.Interfaces.Negocio
{
    public interface IProdutoNegocio
    {
        Task<Produto> CadastrarProduto(ProdutoDTO produto);
    }
}
