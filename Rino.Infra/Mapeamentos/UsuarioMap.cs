using Rino.Dominio.Entidades;
using Rino.Infra.Conversores;

namespace Rino.Infra.Mapeamentos
{
    public static class UsuarioMap
    {
        public static Usuario ToUsuario(this UsuarioFirestore firestoreUsuario)
        {
            return new Usuario
            {
                Id = firestoreUsuario.Id,
                Nome = firestoreUsuario.Nome,
                Email = firestoreUsuario.Email,
                CPF = firestoreUsuario.CPF,
                Ativo = firestoreUsuario.Ativo,
                Criacao = firestoreUsuario.Criacao,
                CadadastradoID = firestoreUsuario.CadadastradoID,
                SenhaHash = firestoreUsuario.SenhaHash
            };
        }

        public static UsuarioFirestore ToFirestoreUsuario(this Usuario usuario)
        {
            return new UsuarioFirestore
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                CPF = usuario.CPF,
                Ativo = usuario.Ativo,
                Criacao = usuario.Criacao,
                CadadastradoID = usuario.CadadastradoID,
                SenhaHash = usuario.SenhaHash
            };
        }
    }
}

