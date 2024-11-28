using ProjetoLogin.Models.Usuarios.Enums;
using ProjetoLogin.Models.Usuarios.Filters;
using ProjetoLogin.Models.Usuarios.ViewModels;

namespace ProjetoLogin.Services.Interfaces
{
	public interface IUsuariosService
    {
	    Task<UsuarioPaginated> GetAllAsync(UsuarioFilter filter);
	    Task<PutUsuario?> GetByIdAsync(int id);
		Task<bool?> CreateAsync(PostUsuario usuario);
        Task<bool?> UpdateAsync(PutUsuario usuario);
        Task<bool?> UpdateStatusAsync(int id, Status status);
        Task<string> GenerateJwtToken(AuthenticateRequest request);
        Task<int?> ValidateJwtToken(string? token);
        Task<bool?> DeleteAsync(int id);
    }
}
