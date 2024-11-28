namespace ProjetoLogin.Models.Usuarios.ViewModels;

public class UsuarioPaginated
{
	public UsuarioPaginated(List<Usuario> users, int totalItems)
	{
		users ??= [];
		Users = users.Select(u => (PutUsuario)u).ToList();
		TotalItems = totalItems;
	}

	public UsuarioPaginated(List<Usuario> users, int totalItems, int currentPage, int totalPages)
	{
		users ??= [];
		Users = users.Select(u => (PutUsuario)u).ToList();
		TotalItems = totalItems;
		CurrentPage = currentPage;
		TotalPages = totalPages;
	}

	public List<PutUsuario> Users { get; set; } = [];

	public int TotalItems { get; set; }

	public int CurrentPage { get; set; } = 1;

	public int TotalPages { get; set; } = 1;
}
