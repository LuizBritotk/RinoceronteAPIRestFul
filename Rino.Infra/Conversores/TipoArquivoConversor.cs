using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Rino.Infra.Conversores
{
    public class TipoArquivoConversor : IFirestoreConverter<TipoArquivoFirestore>
    {
        public TipoArquivoFirestore FromFirestore(object value)
        {
            if (value is IDictionary<string, object> map)
            {
                TipoArquivoFirestore tipoArquivo = new TipoArquivoFirestore();

                if (map.TryGetValue("Alias", out object aliasValue) && aliasValue is string alias)
                    tipoArquivo.Alias = alias;

                if (map.TryGetValue("EntidadeID", out object entidadeIDValue) && entidadeIDValue is string entidadeID)
                    tipoArquivo.EntidadeID = entidadeID;

                if (map.TryGetValue("Extensao", out object extensaoValue) && extensaoValue is string extensao)
                    tipoArquivo.Extensao = extensao;

                if (map.TryGetValue("ID", out object idValue) && idValue is string id)
                    tipoArquivo.ID = id;

                if (map.TryGetValue("Nome", out object nomeValue) && nomeValue is string nome)
                    tipoArquivo.Nome = nome;

                return tipoArquivo;
            }
            throw new ArgumentException("O valor não é um mapa válido", nameof(value));
        }

        public object ToFirestore(TipoArquivoFirestore value)
        {
            return new Dictionary<string, object>
            {
                { "Alias", value.Alias },
                { "EntidadeID", value.EntidadeID },
                { "Extensao", value.Extensao },
                { "ID", value.ID },
                { "Nome", value.Nome }
            };
        }
    }
}
