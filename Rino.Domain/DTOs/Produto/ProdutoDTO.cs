using Rino.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Dominio.DTOs.Produto
{
    public class ProdutoDTO
    {
        public Categoria  Categoria { get; set; }
        public Cliente  Cliente { get; set; }
        public string SkuProduto { get; set; }
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorFaturamento { get; set; }
    }
}
