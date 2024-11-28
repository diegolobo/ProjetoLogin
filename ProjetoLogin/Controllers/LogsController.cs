using Microsoft.AspNetCore.Mvc;

using ProjetoLogin.Models.Usuarios.Enums;
using ProjetoLogin.Utils;
using ProjetoLogin.Utils.Attributes;

using Serilog;

namespace ProjetoLogin.Controllers;

[ApiController]
[Authorize(TipoUsuario.Administrador)]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
	private readonly ILogger<LogsController> _logger;

	public LogsController(ILogger<LogsController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetFile()
	{
		_logger.LogInformation("1 - Obter log's acionado");
		try
		{
			var todayLogName = AppConstants.LogFileName.Replace("-", $"-{DateTime.UtcNow:yyyyMMdd}");
			_logger.LogWarning($"2 - nome atual do arquivo => {todayLogName}");
			var logFilePath1 = Path.Combine(Directory.GetCurrentDirectory(), AppConstants.LogFolder, todayLogName);
			_logger.LogWarning($"3 - caminho do arquivo => {logFilePath1}");

			if (!System.IO.File.Exists(logFilePath1))
			{
				_logger.LogWarning("4 - Arquivo não encontrado");
				var logFilePath2 = Path.Combine("..", Directory.GetCurrentDirectory(), AppConstants.LogFolder);
				_logger.LogWarning($"5 - caminho do arquivo => {logFilePath2}");

				if (!System.IO.File.Exists(logFilePath2))
				{
					_logger.LogWarning("6 - Arquivo não encontrado");
					return BadRequest($"Arquivo não encontrado {logFilePath1};{logFilePath2};");
				}
				logFilePath1 = logFilePath2;
				_logger.LogWarning("7 - Arquivo encontrado");
			}
			_logger.LogWarning("8 - Arquivo encontrado");

			await Log.CloseAndFlushAsync();
			_logger.LogWarning("9 - Log fechado");
			var memory = new MemoryStream();
			await using (var stream = new FileStream(logFilePath1, FileMode.Open, FileAccess.Read))
			{
				await stream.CopyToAsync(memory);
			}

			_logger.LogWarning("10 - Arquivo copiado para a memória");
			memory.Position = 0;

			var fileName = Path.GetFileName(logFilePath1);
			_logger.LogWarning($"11 - nome do arquivo => {fileName}");
			return File(memory, "application/octet-stream", fileName);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Erro ao tentar baixar o arquivo de log");
			return StatusCode(500, "Erro ao baixar o log");
		}
		finally
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.File(
					path: $"{AppConstants.LogFolder}/{AppConstants.LogFileName}",
					rollingInterval: RollingInterval.Day,
					shared: true
				)
				.CreateLogger();
		}
	}
}
