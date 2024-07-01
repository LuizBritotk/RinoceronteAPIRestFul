using Rino.Dominio.Entidades;
using Rino.Infra.Conversores;

namespace Rino.Infra.Mapeamentos
{
    public static class CategoriaMap
    {
        public static Categoria ToCategoria(this CategoriaFirestore firestoreCategoria)
        {
            return new Categoria
            {
                Codigo = firestoreCategoria.Codigo,
                Criacao = firestoreCategoria.Criacao,
                Criador = firestoreCategoria.Criador,
                ID = firestoreCategoria.ID,
                Nome = firestoreCategoria.Nome
            };
        }

        public static CategoriaFirestore ToFirestoreCategoria(this Categoria categoria)
        {
            return new CategoriaFirestore
            {
                Codigo = categoria.Codigo,
                Criacao = categoria.Criacao.ToUniversalTime(),
                Criador = categoria.Criador,
                ID = categoria.ID,
                Nome = categoria.Nome
            };
        }
    }
}
