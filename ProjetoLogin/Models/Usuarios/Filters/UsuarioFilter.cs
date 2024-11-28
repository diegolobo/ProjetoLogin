using ProjetoLogin.Models.Usuarios.Enums;

namespace ProjetoLogin.Models.Usuarios.Filters;

public class UsuarioFilter
{
	public int? Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public TipoUsuario? TipoUsuario { get; set; }
	public Status? Status { get; set; }
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public string Order { get; set; } = string.Empty;
}
