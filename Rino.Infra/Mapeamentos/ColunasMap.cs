using Rino.Dominio.Entidades;
using Rino.Dominio.Enumeradores;
using Rino.Infra.Conversores;

namespace Rino.Infra.Mapeamentos
{
    public static class ColunasMap
    {
        public static Colunas ToColunas(this ColunasFirestore firestoreColunas)
        {
            return new Colunas
            {
                Entidade_ID = firestoreColunas.Entidade_ID,
                ID = firestoreColunas.ID,
                Nome = firestoreColunas.Nome,
                Propriedade_ID = firestoreColunas.Propriedade_ID,
                Tipo = (TipoCampo)firestoreColunas.Tipo,
                Posicao = firestoreColunas.Posicao
            };
        }

        public static ColunasFirestore ToFirestoreColunas(this Colunas colunas)
        {
            return new ColunasFirestore
            {
                Entidade_ID = colunas.Entidade_ID,
                ID = colunas.ID,
                Nome = colunas.Nome,
                Propriedade_ID = colunas.Propriedade_ID,
                Tipo = (TipoCampo)colunas.Tipo,
                Posicao = colunas.Posicao
            };
        }
    }
}
