using Rino.Dominio.Entidades;
using Rino.Infra.Conversores;

namespace Rino.Infra.Mapeamentos
{
    public static class ProdutoMap
    {
        public static Produto ToProduto(this ProdutoFirestore firestoreProduto)
        {
            return new Produto
            {
                ID = firestoreProduto.ID,
                ClienteID = firestoreProduto.ClienteID,
                CategoriaID = firestoreProduto.CategoriaID,
                SkuProduto = firestoreProduto.SkuProduto,
                DataCriacao = firestoreProduto.DataCriacao,
                TotalEstoque = firestoreProduto.TotalEstoque,
                ValorUnitario = firestoreProduto.ValorUnitario,
                Nome = firestoreProduto.Nome
            };
        }

        public static ProdutoFirestore ToFirestoreProduto(Produto produto)
        {
            return new ProdutoFirestore
            {
                ID = produto.ID,
                ClienteID = produto.ClienteID,
                CategoriaID = produto.CategoriaID,
                SkuProduto = produto.SkuProduto,
                DataCriacao = produto.DataCriacao.ToUniversalTime(),
                TotalEstoque = produto.TotalEstoque,
                ValorUnitario = produto.ValorUnitario,
                Nome = produto.Nome
            };
        }
    }
}
