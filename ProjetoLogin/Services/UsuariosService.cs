using Microsoft.IdentityModel.Tokens;

using ProjetoLogin.Database.Repositories.Interfaces;
using ProjetoLogin.Models.Enums;
using ProjetoLogin.Models.ViewModels.Usuarios;
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
		private readonly IUsuariosRepository _usuariosRepository;

        public UsuariosService(
            IUsuariosRepository usuariosRepository,
            IConfiguration configuration)
        {
	        _secret = configuration.GetValue<string>(AppConstants.AppSecretKey) ?? string.Empty;
			_usuariosRepository = usuariosRepository;
        }
        
        public async Task<bool?> CreateAsync(PostUsuario postUsuario)
        {
            try
            {
                if (await _usuariosRepository.CheckCpfCnpjAsync(postUsuario.CpfCnpj))
                    return null;

                return await _usuariosRepository.CreateAsync(postUsuario);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool?> UpdateAsync(PutUsuario putUsuario)
        {
            try
            {
                var usuario = await _usuariosRepository.GetByIdAsync(putUsuario.Id);

                if (usuario == null)
                    return null;

                return await _usuariosRepository.UpdateAsync(putUsuario);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool?> UpdateStatusAsync(int id, Status status)
        {
            try
            {
                var usuario = await _usuariosRepository.GetByIdAsync(id);

                if (usuario == null)
                    return null;

                if (Enum.IsDefined(typeof(Status), status))
                    return null;

                return await _usuariosRepository.UpdateStatusAsync(id, status);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<PutUsuario>> GetAllAsync(Status? status)
        {
            var users = await _usuariosRepository.GetAllAsync(status);
            return users.Select(x => (PutUsuario)x).ToList();
		}

        public async Task<PutUsuario> GetByIdAsync(int id)
        {
            return await _usuariosRepository.GetByIdAsync(id);
        }

        public async Task<string> GenerateJwtToken(AuthenticateRequest request)
        {
            if (!request.IsValid)
                return string.Empty;

            var account = await _usuariosRepository.GetByEmailAsync(request.Username!);

			if (account == null)
                return string.Empty;

			if (!Cryptography.CheckHashPassword(account.Password, request.Password!, account.CpfCnpj))
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
            catch
            {
                return Task.FromResult<int?>(null);
            }
        }
    }
}
