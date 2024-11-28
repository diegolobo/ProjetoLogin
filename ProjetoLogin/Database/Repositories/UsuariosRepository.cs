using Microsoft.EntityFrameworkCore;

using ProjetoLogin.Database.Repositories.Interfaces;
using ProjetoLogin.Models.Usuarios;
using ProjetoLogin.Models.Usuarios.Enums;
using ProjetoLogin.Models.Usuarios.Filters;
using ProjetoLogin.Models.Usuarios.ViewModels;

namespace ProjetoLogin.Database.Repositories
{
	public class UsuariosRepository : IUsuariosRepository
	{
		private readonly LoginContext _context;

		public UsuariosRepository(LoginContext context)
		{
			_context = context;
		}

		public async Task<bool?> CreateAsync(Usuario usuario)
		{
			await _context.Usuarios.AddAsync(usuario);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool?> UpdateAsync(Usuario usuario)
		{
			_context.Usuarios.Update(usuario);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool?> UpdateStatusAsync(int id, Status status)
		{
			try
			{
				var usuario = await GetByIdAsync(id);

				if (usuario == null)
					return null;

				usuario.Status = status;
				_context.Usuarios.Update(usuario);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public async Task<UsuarioPaginated> GetAllAsync(UsuarioFilter filter)
		{
			var query = _context.Usuarios.AsQueryable();

			if (filter.Id is not null)
			{
				var users = query.Where(u => u.Id == filter.Id).ToList();
				return new UsuarioPaginated(users, users.Count);
			}

			if (filter.Status != null)
				query = query.Where(u => u.Status == filter.Status);

			if (!string.IsNullOrEmpty(filter.Nome))
				query = query.Where(u => u.Nome.Contains(filter.Nome));

			if (!string.IsNullOrEmpty(filter.Email))
				query = query.Where(u => u.Email.Contains(filter.Email));

			if (filter.TipoUsuario != null)
				query = query.Where(u => u.TipoUsuario == filter.TipoUsuario);

			var usersList = await query.ToListAsync();
			var total = usersList.Count;
			var totalPages = (int)Math.Ceiling(total / (double)filter.PageSize);

			return new UsuarioPaginated(usersList, total, filter.PageNumber, totalPages);
		}

		public async Task<bool> CheckEmailAsync(string email)
		{
			return await _context.Usuarios.AnyAsync(u => u.Email == email);
		}

		public async Task<Usuario?> GetByIdAsync(int id)
		{
			return await _context.Usuarios.FindAsync(id);
		}

		public async Task<Usuario?> GetByEmailAsync(string email)
		{
			return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<Usuario?> GetUserAsync(string username, string password)
		{
			return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == username && u.Senha == password);
		}

		public async Task<bool?> DeleteAsync(int id)
		{
			var usuario = await _context.Usuarios.FindAsync(id);

			if (usuario == null)
				return null;

			_context.Usuarios.Remove(usuario);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
