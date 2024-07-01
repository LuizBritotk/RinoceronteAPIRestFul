

namespace Rino.Dominio.Entidades
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public bool Ativo { get; set; }
        public DateTime Criacao { get; set; }
        public string CadadastradoID { get; set; }
        public string SenhaHash { get; set; }
    }
}
