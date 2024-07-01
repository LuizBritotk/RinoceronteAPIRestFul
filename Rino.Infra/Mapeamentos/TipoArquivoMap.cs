using Rino.Dominio.Entidades;
using Rino.Infra.Conversores;

namespace Rino.Infra.Mapeamentos
{
    public static class TipoArquivoMap
    {
        public static TipoArquivo ToTipoArquivo(this TipoArquivoFirestore firestoreTipoArquivo)
        {
            return new TipoArquivo
            {
                Alias = firestoreTipoArquivo.Alias,
                EntidadeID = firestoreTipoArquivo.EntidadeID,
                Extensao = firestoreTipoArquivo.Extensao,
                ID = firestoreTipoArquivo.ID,
                Nome = firestoreTipoArquivo.Nome
            };
        }

        public static TipoArquivoFirestore ToFirestoreTipoArquivo(this TipoArquivo tipoArquivo)
        {
            return new TipoArquivoFirestore
            {
                Alias = tipoArquivo.Alias,
                EntidadeID = tipoArquivo.EntidadeID,
                Extensao = tipoArquivo.Extensao,
                ID = tipoArquivo.ID,
                Nome = tipoArquivo.Nome
            };
        }
    }
}
