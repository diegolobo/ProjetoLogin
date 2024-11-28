using Microsoft.EntityFrameworkCore;

using ProjetoLogin.Database;
using ProjetoLogin.Database.Repositories;
using ProjetoLogin.Database.Repositories.Interfaces;
using ProjetoLogin.Models.Usuarios;
using ProjetoLogin.Models.Usuarios.Enums;
using ProjetoLogin.Services;
using ProjetoLogin.Services.Interfaces;
using ProjetoLogin.Utils;
using ProjetoLogin.Utils.Extensions;
using ProjetoLogin.Utils.Middlewares;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddDefaultLogging();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration(AppConstants.ApplicationVersion);
builder.Services.AddDbContext<LoginContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString(AppConstants.ConnectionStringName),
	    new MySqlServerVersion(AppConstants.MySqlServerVersion)));

builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var repository = scope.ServiceProvider.GetRequiredService<IUsuariosRepository>();

	if (!await repository.CheckEmailAsync(AppConstants.AdminCpf))
	{
		var service = scope.ServiceProvider.GetRequiredService<IUsuariosService>();
		await service.CreateAsync(new Usuario
		{
			Nome = AppConstants.AdminNome,
			Status = Status.Ativo,
			TipoUsuario = TipoUsuario.Administrador,
			Email = AppConstants.AdminEmail,
			Senha = builder.Configuration.GetValue<string>(AppConstants.AdminPasswordKey)!
		});
	}
}

app.UseSwaggerSetup(AppConstants.ApplicationName);
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();

app.Run();
