using ProjetoLogin.Models.Usuarios.Enums;
using ProjetoLogin.Utils;

using System.ComponentModel.DataAnnotations;

namespace ProjetoLogin.Models.Usuarios.ViewModels
{
	public class PostUsuario
	{
		[Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
		[StringLength(Usuario.NomeMaxLength, ErrorMessage = "O campo Nome Completo deve ter no máximo 100 caracteres.")]
		public string Nome { get; set; }

		[Required(ErrorMessage = "O campo Email é obrigatório.")]
		[StringLength(Usuario.EmailMaxLength, ErrorMessage = "O campo Email deve ter no máximo 100 caracteres.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "O campo Senha é obrigatório.")]
		[StringLength(Usuario.PasswordMaxLength)]
		public string Password { get; set; }

		[Required(ErrorMessage = "O campo Tipo de Usuário é obrigatório.")]
		[EnumDataType(typeof(TipoUsuario), ErrorMessage = "O campo Tipo de Usuário é inválido.")]
		public TipoUsuario TipoUsuario { get; set; }

		[Required(ErrorMessage = "O campo Status é obrigatório.")]
		[EnumDataType(typeof(Status), ErrorMessage = "O campo Status é inválido.")]
		public Status Status { get; set; }

		public static implicit operator Usuario(PostUsuario postUsuario)
		{
			return new()
			{
				Nome = postUsuario.Nome,
				Email = postUsuario.Email,
				Senha = Cryptography.GetHashPassword(postUsuario.Password, postUsuario.Email),
				TipoUsuario = postUsuario.TipoUsuario,
				Status = postUsuario.Status
			};
		}

		public static implicit operator PostUsuario(Usuario usuario)
		{
			return new()
			{
				Email = usuario.Email,
				Nome = usuario.Nome,
				Status = usuario.Status,
				TipoUsuario = usuario.TipoUsuario,
				Password = usuario.Senha
			};
		}
	}
}
