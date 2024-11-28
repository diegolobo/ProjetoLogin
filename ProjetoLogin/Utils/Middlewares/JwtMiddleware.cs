using ProjetoLogin.Services.Interfaces;

namespace ProjetoLogin.Utils.Middlewares;

public class JwtMiddleware
{
	private readonly RequestDelegate _next;

	public JwtMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context, IUsuariosService userService)
	{
		var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		var userId = await userService.ValidateJwtToken(token);
		if (userId != null)
		{
			context.Items["User"] = await userService.GetByIdAsync(userId.Value);
		}

		await _next(context);
	}
}
