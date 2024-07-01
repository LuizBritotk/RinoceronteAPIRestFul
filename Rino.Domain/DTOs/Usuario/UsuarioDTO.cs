
namespace Rino.Dominio.DTOs.Usuario
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public DateTime Criacao { get; set; }
        public string Login { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public string Token { get; set; }
    }
}
