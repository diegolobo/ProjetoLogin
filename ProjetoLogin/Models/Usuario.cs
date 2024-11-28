using ProjetoLogin.Models.Enums;

namespace ProjetoLogin.Models
{
    public class Usuario : Pessoa
    {
        public const int EmailMaxLength = 100;
        public const int PasswordMaxLength = 25;
        public const int PasswordHashMaxLength = 500;

        public string Email { get; set; }
        public string Password { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public Status Status { get; set; }
    }
}
