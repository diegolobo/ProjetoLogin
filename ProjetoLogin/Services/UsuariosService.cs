using Microsoft.IdentityModel.Tokens;

using ProjetoLogin.Database.Repositories.Interfaces;
using ProjetoLogin.Models.Usuarios.Enums;
using ProjetoLogin.Models.Usuarios.Filters;
using ProjetoLogin.Models.Usuarios.ViewModels;
using ProjetoLogin.Services.Interfaces;
using ProjetoLogin.Utils;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetoLogin.Services
{
	public class UsuariosService : IUsuariosService
	{
		private readonly string _secret;
		private readonly ILogger<UsuariosService> _logger;
		private readonly IUsuariosRepository _repository;

		public UsuariosService(
			ILogger<UsuariosService> logger,
			IUsuariosRepository repository,
			IConfiguration configuration)
		{
			_secret = configuration.GetValue<string>(AppConstants.AppSecretKey) ?? string.Empty;
			_logger = logger;
			_repository = repository;
		}

		public async Task<UsuarioPaginated> GetAllAsync(UsuarioFilter filter)
		{
			return await _repository.GetAllAsync(filter);
		}

		public async Task<PutUsuario?> GetByIdAsync(int id)
		{
			return await _repository.GetByIdAsync(id);
		}

		public async Task<bool?> CreateAsync(PostUsuario postUsuario)
		{
			try
			{
				if (await _repository.CheckEmailAsync(postUsuario.Email))
					return null;

				return await _repository.CreateAsync(postUsuario);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Erro ao criar usuário");
				return false;
			}
		}

		public async Task<bool?> UpdateAsync(PutUsuario putUsuario)
		{
			try
			{
				var usuario = await _repository.GetByIdAsync(putUsuario.Id);

				if (usuario == null)
					return null;

				return await _repository.UpdateAsync(putUsuario);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Erro ao atualizar usuário");
				return false;
			}
		}

		public async Task<bool?> UpdateStatusAsync(int id, Status status)
		{
			try
			{
				var usuario = await _repository.GetByIdAsync(id);

				if (usuario == null)
					return null;

				if (Enum.IsDefined(typeof(Status), status))
					return null;

				return await _repository.UpdateStatusAsync(id, status);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Erro ao atualizar status do usuário");
				return false;
			}
		}

		public async Task<string> GenerateJwtToken(AuthenticateRequest request)
		{
			if (!request.IsValid)
				return string.Empty;

			var account = await _repository.GetByEmailAsync(request.Username!);

			if (account == null)
				return string.Empty;

			if (!Cryptography.CheckHashPassword(account.Senha, request.Password!, account.Email))
				return string.Empty;

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim(AppConstants.ClaimIdType, account.Id.ToString()) }),
				Expires = DateTime.UtcNow.AddMinutes(30),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public Task<int?> ValidateJwtToken(string? token)
		{
			if (token == null)
				return Task.FromResult<int?>(null);

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_secret);
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == AppConstants.ClaimIdType).Value);

				return Task.FromResult<int?>(accountId);
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Token inválido => {token}");
				return Task.FromResult<int?>(null);
			}
		}

		public Task<bool?> DeleteAsync(int id)
		{
			return _repository.DeleteAsync(id);
		}
	}
}
