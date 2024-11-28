using ProjetoLogin.Models.Usuarios.Enums;

namespace ProjetoLogin.Models.Usuarios
{
	public class Usuario
	{
		public const short NomeMaxLength = 100;
		public const short EmailMaxLength = 100;
		public const short PasswordMaxLength = 25;
		public const short PasswordHashMaxLength = 500;

		public int Id { get; set; }
		public string Nome { get; set; }
		public string Email { get; set; }
		public string Senha { get; set; }
		public TipoUsuario TipoUsuario { get; set; }
		public Status Status { get; set; }
	}
}
