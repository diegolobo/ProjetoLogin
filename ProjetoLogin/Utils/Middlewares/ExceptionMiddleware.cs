using System.ComponentModel.DataAnnotations;
using System.Text.Json;

using Serilog;

namespace ProjetoLogin.Utils.Middlewares;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;

	public ExceptionMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (ValidationException ex)
		{
			await HandleValidationExceptionAsync(context, ex);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		Log.Logger.Error(exception, $"Unknown error with RequestId => {context.TraceIdentifier}: {exception.Message}.");
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = StatusCodes.Status500InternalServerError;

		var response = new
		{
			Success = false,
			Message = "Erro interno do servidor",
			Error = "Erro interno do servidor, por favor tente novamente.",
			Detail = $"Se o erro persistir, contate o suporte com o seguinte RequestId => {context.TraceIdentifier}."
		};

		var jsonOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
	}

	private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = StatusCodes.Status400BadRequest;

		var response = new
		{
			Success = false,
			Message = "Dados inválidos",
			Error = exception
		};

		var jsonOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
	}
}
