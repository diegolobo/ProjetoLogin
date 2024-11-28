using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ProjetoLogin.Models.Enums;

namespace ProjetoLogin.Models.ViewModels.Usuarios
{
    public class PutUsuario
    {
        [IgnoreDataMember]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
        public string NomeCommpleto { get; set; }

        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [StringLength(Usuario.EmailMaxLength, ErrorMessage = "O campo Email deve ter no máximo 100 caracteres.")]
        public string Email { get; set; }

		[Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Usuário é obrigatório.")]
        [EnumDataType(typeof(TipoUsuario), ErrorMessage = "O campo Tipo de Usuário é inválido.")]
        public TipoUsuario TipoUsuario { get; set; }

        public static implicit operator PutUsuario(Usuario usuario)
		{
			return new PutUsuario
			{
				Id = usuario.Id,
				NomeCommpleto = usuario.NomeCommpleto,
				DataNascimento = usuario.DataNascimento,
				Email = usuario.Email,
				Telefone = usuario.Telefone,
				TipoUsuario = usuario.TipoUsuario
			};
		}

		public static implicit operator Usuario(PutUsuario putUsuario)
		{
			return new()
			{
				Id = putUsuario.Id,
				NomeCommpleto = putUsuario.NomeCommpleto,
				DataNascimento = putUsuario.DataNascimento,
				Email = putUsuario.Email,
				Telefone = putUsuario.Telefone,
				TipoUsuario = putUsuario.TipoUsuario
			};
		}
	}
}
