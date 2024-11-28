using System.ComponentModel.DataAnnotations;

namespace ProjetoLogin.Models.Usuarios.ViewModels;

public class AuthenticateRequest
{
	[Required]
	public string? Username { get; set; }

	[Required]
	public string? Password { get; set; }

	public bool IsValid => Validate();

	private bool Validate()
	{
		return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
	}
}
