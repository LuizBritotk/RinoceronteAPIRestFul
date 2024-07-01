using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Rino.Infra.Conversores
{
    public class ClienteConversor : IFirestoreConverter<ClienteFirestore>
    {
        private readonly FirestoreDateTimeConversor _dateTimeConverter = new FirestoreDateTimeConversor();

        public ClienteFirestore FromFirestore(object value)
        {
            if (value is IDictionary<string, object> map)
            {
                ClienteFirestore cliente = new ClienteFirestore();

                if (map.TryGetValue("CPF", out object cpfValue) && cpfValue is string cpf)
                    cliente.CPF = cpf;

                if (map.TryGetValue("CodigoCliente", out object codigoClienteValue) && codigoClienteValue is string codigoCliente)
                    cliente.CodigoCliente = codigoCliente;

                if (map.TryGetValue("Criacao", out object criacaoValue) && criacaoValue is Timestamp criacao)
                    cliente.Criacao = criacao.ToDateTime();

                if (map.TryGetValue("Data", out object dataValue) && dataValue is Timestamp data)
                    cliente.Data = data.ToDateTime();

                if (map.TryGetValue("ID", out object idValue) && idValue is string id)
                    cliente.ID = id;

                if (map.TryGetValue("Nome", out object nomeValue) && nomeValue is string nome)
                    cliente.Nome = nome;

                return cliente;
            }
            throw new ArgumentException("O valor não é um mapa válido", nameof(value));
        }

        public object ToFirestore(ClienteFirestore value)
        {
            return new Dictionary<string, object>
            {
                { "CPF", value.CPF },
                { "CodigoCliente", value.CodigoCliente },
                { "Criacao", _dateTimeConverter.ToFirestore(value.Criacao) },
                { "Data", _dateTimeConverter.ToFirestore(value.Data) },
                { "ID", value.ID },
                { "Nome", value.Nome }
            };
        }
    }
}
