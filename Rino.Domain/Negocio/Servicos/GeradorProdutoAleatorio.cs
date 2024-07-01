using System;
using Rino.Dominio.DTOs.Produto;
using Rino.Dominio.Entidades;

namespace Rino.Dominio.Negocio.Servicos
{
    public class GeradorProdutoAleatorio
    {
        #region Métodos Privados

        private static Random random = new Random();
        private static string[] adjetivos = { "Elegante", "Confortável", "Moderno", "Clássico", "Durável", "Estiloso", "Esportivo", "Luxuoso", "Casual", "Vibrante" };
        private static string[] categorias = { "Camiseta", "Calça", "Vestido", "Blusa", "Casaco", "Sapato", "Tênis", "Bolsa", "Chapéu", "Acessório" };

        private static string GerarNome()
        {
            string adjetivo = adjetivos[random.Next(adjetivos.Length)];
            string categoria = categorias[random.Next(categorias.Length)];
            return $"{adjetivo} {categoria}";
        }

        #endregion

        #region Métodos Públicos

        public Produto GerarProdutoAleatorio(Categoria categoria, Cliente cliente, ProdutoDTO produtoDTO)
        {
            string nome = GerarNome();
            return new Produto
            {
                ID = Guid.NewGuid().ToString(),
                CategoriaID = categoria.ID,
                ClienteID = cliente.ID,
                SkuProduto = produtoDTO.SkuProduto,
                DataCriacao = DateTime.Now,
                TotalEstoque = produtoDTO.Quantidade,
                Nome = nome,
                ValorUnitario = (produtoDTO.Quantidade > 0 ? (produtoDTO.ValorFaturamento / produtoDTO.Quantidade).ToString("F2") : "0")
            };
        }

        #endregion
    }
}
