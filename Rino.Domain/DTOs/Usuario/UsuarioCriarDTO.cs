namespace Rino.Dominio.DTOs.Usuario
{
    public class UsuarioCriarDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public bool Ativo { get; set; }
        public string Login { get; set; }
        public string SenhaHASH { get; set; }
    }
}
