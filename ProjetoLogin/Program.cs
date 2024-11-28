using Microsoft.EntityFrameworkCore;

using ProjetoLogin.Database;
using ProjetoLogin.Database.Repositories;
using ProjetoLogin.Database.Repositories.Interfaces;
using ProjetoLogin.Middlewares;
using ProjetoLogin.Services;
using ProjetoLogin.Services.Interfaces;
using ProjetoLogin.Utils;

using System.Text.Json.Serialization;
using ProjetoLogin.Models;
using ProjetoLogin.Models.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration("1.0");
builder.Services.AddDbContext<LoginContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DbProjetoLogin"),
	    new MySqlServerVersion("8.0.30")));

builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var repository = scope.ServiceProvider.GetRequiredService<IUsuariosRepository>();

	if (!await repository.CheckCpfCnpjAsync(AppConstants.AdminCpf))
	{
		var service = scope.ServiceProvider.GetRequiredService<IUsuariosService>();
		await service.CreateAsync(new Usuario()
		{
			CpfCnpj = AppConstants.AdminCpf,
			NomeCommpleto = AppConstants.AdminNome,
			DataNascimento = AppConstants.AdminDataNascimento,
			Status = Status.Ativo,
			TipoUsuario = TipoUsuario.Administrador,
			Email = AppConstants.AdminEmail,
			Telefone = AppConstants.AdminTelefone,
			Password = builder.Configuration.GetValue<string>(AppConstants.AdminPasswordKey)!
		});
	}
}

app.UseSwaggerSetup("Projeto Login API");
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();

app.Run();
