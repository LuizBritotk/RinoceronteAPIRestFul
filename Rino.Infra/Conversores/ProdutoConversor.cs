using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Rino.Infra.Conversores
{
    public class ProdutoConversor : IFirestoreConverter<ProdutoFirestore>
    {
        private readonly FirestoreDateTimeConversor _dateTimeConverter = new FirestoreDateTimeConversor();

        public ProdutoFirestore FromFirestore(object value)
        {
            if (value is IDictionary<string, object> map)
            {
                ProdutoFirestore produto = new ProdutoFirestore();

                if (map.TryGetValue("ID", out object idValue) && idValue is string id)
                    produto.ID = id;

                if (map.TryGetValue("ClienteID", out object clienteIdValue) && clienteIdValue is string clienteId)
                    produto.ClienteID = clienteId;

                if (map.TryGetValue("CategoriaID", out object categoriaIdValue) && categoriaIdValue is string categoriaId)
                    produto.CategoriaID = categoriaId;

                if (map.TryGetValue("SkuProduto", out object skuValue) && skuValue is string sku)
                    produto.SkuProduto = sku;

                if (map.TryGetValue("DataCriacao", out object dataCriacaoValue) && dataCriacaoValue is Timestamp dataCriacao)
                    produto.DataCriacao = dataCriacao.ToDateTime();

                if (map.TryGetValue("TotalEstoque", out object totalEstoqueValue) && totalEstoqueValue is long totalEstoque)
                    produto.TotalEstoque = (int)totalEstoque;
               
                if (map.TryGetValue("ValorUnitario", out object valorUnitarioValue) && valorUnitarioValue is decimal valorUnitario)
                    produto.ValorUnitario = valorUnitario.ToString(); 


                if (map.TryGetValue("Nome", out object nomeValue) && nomeValue is string nome)
                    produto.Nome = nome;

                return produto;
            }
            throw new ArgumentException("O valor não é um mapa válido", nameof(value));
        }

        public object ToFirestore(ProdutoFirestore value)
        {
            return new Dictionary<string, object>
            {
                { "ID", value.ID },
                { "ClienteID", value.ClienteID },
                { "CategoriaID", value.CategoriaID },
                { "SkuProduto", value.SkuProduto },
                { "DataCriacao", _dateTimeConverter.ToFirestore(value.DataCriacao) },
                { "TotalEstoque", value.TotalEstoque },
                { "ValorUnitario", value.ValorUnitario },
                { "Nome", value.Nome }
            };
        }
    }
}
