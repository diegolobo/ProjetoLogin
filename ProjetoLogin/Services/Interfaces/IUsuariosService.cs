using ProjetoLogin.Models.Enums;
using ProjetoLogin.Models.ViewModels.Usuarios;

namespace ProjetoLogin.Services.Interfaces
{
	public interface IUsuariosService
    {
        Task<bool?> CreateAsync(PostUsuario usuario);
        Task<bool?> UpdateAsync(PutUsuario usuario);
        Task<bool?> UpdateStatusAsync(int id, Status status);
        Task<List<PutUsuario>> GetAllAsync(Status? status);
        Task<PutUsuario> GetByIdAsync(int id);
        Task<string> GenerateJwtToken(AuthenticateRequest request);
        Task<int?> ValidateJwtToken(string? token);
    }
}
