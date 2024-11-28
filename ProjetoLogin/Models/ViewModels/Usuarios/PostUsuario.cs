using ProjetoLogin.Models.Enums;
using ProjetoLogin.Utils;

using System.ComponentModel.DataAnnotations;

namespace ProjetoLogin.Models.ViewModels.Usuarios
{
	public class PostUsuario
    {
        [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
        [StringLength(Pessoa.NomeMaxLength, ErrorMessage = "O campo Nome Completo deve ter no máximo 100 caracteres.")]
        public string NomeCommpleto { get; set; }

        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo CPF/CNPJ é obrigatório.")]
        [StringLength(Pessoa.CpfCnpjMaxLength, ErrorMessage = "O campo CPF/CNPJ deve ter no máximo 21 caracteres.")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [StringLength(Pessoa.TelefoneMaxLength, ErrorMessage = "O campo Telefone deve ter no máximo 15 caracteres.")]
        public string Telefone { get; set; }

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
			return new ()
			{
				NomeCommpleto = postUsuario.NomeCommpleto,
				DataNascimento = postUsuario.DataNascimento,
				CpfCnpj = postUsuario.CpfCnpj,
				Telefone = postUsuario.Telefone,
				Email = postUsuario.Email,
				Password = Cryptography.GetHashPassword(postUsuario.Password, postUsuario.CpfCnpj),
				TipoUsuario = postUsuario.TipoUsuario,
				Status = postUsuario.Status
			};
		}

		public static implicit operator PostUsuario(Usuario usuario)
		{
			return new()
			{
				CpfCnpj = usuario.CpfCnpj,
				DataNascimento = usuario.DataNascimento,
				Email = usuario.Email,
				NomeCommpleto = usuario.NomeCommpleto,
				Status = usuario.Status,
				Telefone = usuario.Telefone,
				TipoUsuario = usuario.TipoUsuario,
				Password = usuario.Password
			};
		}
	}
}
