using Google.Cloud.Firestore;
using Rino.Dominio.Enumeradores;
using System;
using System.Collections.Generic;

namespace Rino.Infra.Conversores
{
    public class ColunasConversor : IFirestoreConverter<ColunasFirestore>
    {
        public ColunasFirestore FromFirestore(object value)
        {
            if (value is IDictionary<string, object> map)
            {
                ColunasFirestore colunas = new ColunasFirestore();

                if (map.TryGetValue("Entidade_ID", out object entidadeIDValue) && entidadeIDValue is string entidadeID)
                    colunas.Entidade_ID = entidadeID;

                if (map.TryGetValue("ID", out object idValue) && idValue is string id)
                    colunas.ID = id;

                if (map.TryGetValue("Nome", out object nomeValue) && nomeValue is string nome)
                    colunas.Nome = nome;

                if (map.TryGetValue("Propriedade_ID", out object propriedadeIDValue) && propriedadeIDValue is string propriedadeID)
                    colunas.Propriedade_ID = propriedadeID;

                if (map.TryGetValue("Tipo", out object tipoValue) && tipoValue is long tipo)
                    colunas.Tipo = (TipoCampo)tipo;

                if (map.TryGetValue("Posicao", out object posicaoValue) && posicaoValue is long posicao)
                    colunas.Posicao = (int)posicao;

                return colunas;
            }
            throw new ArgumentException("O valor não é um mapa válido", nameof(value));
        }

        public object ToFirestore(ColunasFirestore value)
        {
            return new Dictionary<string, object>
            {
                { "Entidade_ID", value.Entidade_ID },
                { "ID", value.ID },
                { "Nome", value.Nome },
                { "Propriedade_ID", value.Propriedade_ID },
                { "Tipo", (int)value.Tipo },
                { "Posicao", value.Posicao }
            };
        }
    }
}
