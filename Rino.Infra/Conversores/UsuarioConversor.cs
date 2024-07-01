using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Rino.Infra.Conversores
{
    public class UsuarioConversor : IFirestoreConverter<UsuarioFirestore>
    {
        private readonly FirestoreDateTimeConversor _dateTimeConverter = new FirestoreDateTimeConversor();

        public UsuarioFirestore FromFirestore(object value)
        {
            if (value is IDictionary<string, object> map)
            {
                UsuarioFirestore usuario = new UsuarioFirestore();

                if (map.TryGetValue("ID", out object idValue) && idValue is string id)
                    usuario.Id = id;

                if (map.TryGetValue("Nome", out object nomeValue) && nomeValue is string nome)
                    usuario.Nome = nome;

                if (map.TryGetValue("Email", out object emailValue) && emailValue is string email)
                    usuario.Email = email;

                if (map.TryGetValue("CPF", out object cpfValue) && cpfValue is string cpf)
                    usuario.CPF = cpf;

                if (map.TryGetValue("Ativo", out object ativoValue) && ativoValue is bool ativo)
                    usuario.Ativo = ativo;

                if (map.TryGetValue("Criacao", out object criacaoValue) && criacaoValue is DateTime criacao)
                    usuario.Criacao = criacao;

                if (map.TryGetValue("CadadastradoID", out object cadastradoIdValue) && cadastradoIdValue is string cadastradoId)
                    usuario.CadadastradoID = cadastradoId;

                if (map.TryGetValue("SenhaHash", out object senhaHashValue) && senhaHashValue is string senhaHash)
                    usuario.SenhaHash = senhaHash;

                return usuario;
            }
            throw new ArgumentException("O valor não é um mapa válido", nameof(value));
        }

        public object ToFirestore(UsuarioFirestore value)
        {
            return new Dictionary<string, object>
            {
                { "Id", value.Id },
                { "Nome", value.Nome },
                { "Email", value.Email },
                { "CPF", value.CPF },
                { "Ativo", value.Ativo },
                { "Criacao", _dateTimeConverter.ToFirestore(value.Criacao) },
                { "CadadastradoID", value.CadadastradoID },
                { "SenhaHash", value.SenhaHash }
            };
        }
    }
}
