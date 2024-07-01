using Google.Cloud.Firestore;
using Rino.Dominio.Entidades;

namespace Rino.Infra.Conversores
{
    [FirestoreData(ConverterType = typeof(TipoArquivoConversor))]
    public class TipoArquivoFirestore : TipoArquivo
    {
    }
}
