using ProjetoLogin.Models.Usuarios;
using ProjetoLogin.Models.Usuarios.Enums;
using ProjetoLogin.Models.Usuarios.Filters;
using ProjetoLogin.Models.Usuarios.ViewModels;

namespace ProjetoLogin.Database.Repositories.Interfaces
{
	public interface IUsuariosRepository
    {
	    Task<UsuarioPaginated> GetAllAsync(UsuarioFilter filter);
	    Task<Usuario?> GetByIdAsync(int id);
	    Task<Usuario?> GetByEmailAsync(string email);
		Task<bool?> CreateAsync(Usuario usuario);
        Task<bool?> UpdateAsync(Usuario usuario);
        Task<bool?> UpdateStatusAsync(int id, Status status);
        Task<bool> CheckEmailAsync(string cpfCnpj);
        Task<bool?> DeleteAsync(int id);
    }
}
