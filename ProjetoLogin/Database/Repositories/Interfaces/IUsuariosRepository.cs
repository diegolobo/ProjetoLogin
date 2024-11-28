using ProjetoLogin.Models.Enums;
using ProjetoLogin.Models;

namespace ProjetoLogin.Database.Repositories.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<bool?> CreateAsync(Usuario usuario);
        Task<bool?> UpdateAsync(Usuario usuario);
        Task<bool?> UpdateStatusAsync(int id, Status status);
        Task<List<Usuario>> GetAllAsync(Status? status);
        Task<bool> CheckCpfCnpjAsync(string cpfCnpj);
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetUserAsync(string username, string password);
        Task<bool?> DeleteAsync(int id);
    }
}
