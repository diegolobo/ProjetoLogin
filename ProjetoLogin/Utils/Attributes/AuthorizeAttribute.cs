using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using ProjetoLogin.Models.Usuarios.Enums;
using ProjetoLogin.Models.Usuarios.ViewModels;

namespace ProjetoLogin.Utils.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
	private const string UserItem = "User";
	private const string UnauthorizedMessage = "Não autorizado";
	public TipoUsuario[]? TipoUsuario;

	public AuthorizeAttribute(params TipoUsuario[] tipoUsuario)
	{
		TipoUsuario = tipoUsuario;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
		if (allowAnonymous)
			return;

		var account = (PutUsuario?)context.HttpContext.Items[UserItem];
		if (account == null)
		{
			context.Result = new JsonResult(new { message = UnauthorizedMessage }) { StatusCode = StatusCodes.Status401Unauthorized };
			return;
		}

		if (account.TipoUsuario == Models.Usuarios.Enums.TipoUsuario.Administrador) return;

		if (context.ActionDescriptor.EndpointMetadata
			.OfType<AuthorizeAttribute>()
			.Any(r
				=> r.TipoUsuario != null
				   && r.TipoUsuario.Any(role => role == account.TipoUsuario))) return;

		context.Result = new JsonResult(new { message = UnauthorizedMessage }) { StatusCode = StatusCodes.Status401Unauthorized };
	}
}
