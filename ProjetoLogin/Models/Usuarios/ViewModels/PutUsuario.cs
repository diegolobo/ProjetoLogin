using ProjetoLogin.Models.Usuarios.Enums;

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ProjetoLogin.Models.Usuarios.ViewModels
{
	public class PutUsuario
	{
		[IgnoreDataMember]
		public int Id { get; set; }

		[Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
		public string Nome { get; set; }

		[Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
		public DateTime DataNascimento { get; set; }

		[Required(ErrorMessage = "O campo Email é obrigatório.")]
		[StringLength(Usuario.EmailMaxLength, ErrorMessage = "O campo Email deve ter no máximo 100 caracteres.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "O campo Tipo de Usuário é obrigatório.")]
		[EnumDataType(typeof(TipoUsuario), ErrorMessage = "O campo Tipo de Usuário é inválido.")]
		public TipoUsuario TipoUsuario { get; set; }

		public Status Status { get; set; }

		public static implicit operator PutUsuario(Usuario? usuario)
		{
			if (usuario is null) return new PutUsuario();

			return new PutUsuario
			{
				Id = usuario.Id,
				Nome = usuario.Nome,
				Email = usuario.Email,
				TipoUsuario = usuario.TipoUsuario,
				Status = usuario.Status
			};
		}

		public static implicit operator Usuario(PutUsuario putUsuario)
		{
			return new()
			{
				Id = putUsuario.Id,
				Nome = putUsuario.Nome,
				Email = putUsuario.Email,
				TipoUsuario = putUsuario.TipoUsuario,
				Status = putUsuario.Status
			};
		}
	}
}
