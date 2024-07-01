namespace Rino.Dominio.Util
{
    public class RespostaPadrao
    {
        public string Mensagem { get; set; }
        public bool Error { get; set; }
        public int HttpStatusCode { get; set; }
        public object Dados { get; set; } 

        public RespostaPadrao(string mensagem, bool error, int httpStatusCode, object dados = null)
        {
            Mensagem = mensagem;
            Error = error;
            HttpStatusCode = httpStatusCode;
            Dados = dados;
        }
    }
}
