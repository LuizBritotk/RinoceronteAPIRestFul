using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Rino.Infra.Conversores
{
    public class CategoriaConversor : IFirestoreConverter<CategoriaFirestore>
    {
        private readonly FirestoreDateTimeConversor _dateTimeConverter = new FirestoreDateTimeConversor();

        public CategoriaFirestore FromFirestore(object value)
        {
            if (value is IDictionary<string, object> map)
            {
                CategoriaFirestore categoria = new CategoriaFirestore();

                if (map.TryGetValue("Codigo", out object codigoValue) && codigoValue is string codigo)
                    categoria.Codigo = codigo;

                if (map.TryGetValue("Criacao", out object criacaoValue) && criacaoValue is Timestamp criacao)
                    categoria.Criacao = criacao.ToDateTime();

                if (map.TryGetValue("Criador", out object criadorValue) && criadorValue is string criador)
                    categoria.Criador = criador;

                if (map.TryGetValue("ID", out object idValue) && idValue is string id)
                    categoria.ID = id;

                if (map.TryGetValue("Nome", out object nomeValue) && nomeValue is string nome)
                    categoria.Nome = nome;

                return categoria;
            }
            throw new ArgumentException("O valor não é um mapa válido", nameof(value));
        }

        public object ToFirestore(CategoriaFirestore value)
        {
            return new Dictionary<string, object>
            {
                { "Codigo", value.Codigo },
                { "Criacao", _dateTimeConverter.ToFirestore(value.Criacao) },
                { "Criador", value.Criador },
                { "ID", value.ID },
                { "Nome", value.Nome }
            };
        }
    }
}
