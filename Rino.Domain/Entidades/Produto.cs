using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Dominio.Entidades
{
    public class Produto
    {
        public string ID { get; set; }
        public string ClienteID { get; set; }
        public string CategoriaID { get; set; }
        public string SkuProduto { get; set; }
        public DateTime DataCriacao { get; set; }
        public int TotalEstoque { get; set; }
        public string ValorUnitario { get; set; }
        public string Nome { get; set; }

    }
}
