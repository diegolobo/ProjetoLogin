using Serilog;

namespace ProjetoLogin.Utils.Extensions;

public static class LoggingExtension
{
	public static WebApplicationBuilder AddDefaultLogging(this WebApplicationBuilder builder)
	{
		Log.Logger = new LoggerConfiguration().CreateLogger();
		builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
		{
			loggerConfiguration
				.ReadFrom.Configuration(hostingContext.Configuration)
				.Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
				.Enrich.WithProperty("Application", AppConstants.ApplicationName)
				.Enrich.FromLogContext()
				.WriteTo.Console
				(
					outputTemplate:
					"{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}"
				)
				.WriteTo.File($"{AppConstants.LogFolder}/{AppConstants.LogFileName}",
					rollingInterval: RollingInterval.Day,
					shared: true,
					retainedFileCountLimit: 10,
					outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}");

		});

		builder.Services.AddLogging();

		return builder;
	}
}
