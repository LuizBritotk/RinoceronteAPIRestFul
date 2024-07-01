using Rino.Dominio.Entidades;
using Rino.Infra.Conversores;

namespace Rino.Infra.Mapeamentos
{
    public static class ClienteMap
    {
        public static Cliente ToCliente(this ClienteFirestore firestoreCliente)
        {
            return new Cliente
            {
                CPF = firestoreCliente.CPF,
                CodigoCliente = firestoreCliente.CodigoCliente,
                Criacao = firestoreCliente.Criacao,
                Data = firestoreCliente.Data,
                ID = firestoreCliente.ID,
                Nome = firestoreCliente.Nome
            };
        }

        public static ClienteFirestore ToFirestoreCliente(this Cliente cliente)
        {
            return new ClienteFirestore
            {
                CPF = cliente.CPF,
                CodigoCliente = cliente.CodigoCliente,
                Criacao = cliente.Criacao,
                Data = cliente.Data,
                ID = cliente.ID,
                Nome = cliente.Nome
            };
        }
    }
}
