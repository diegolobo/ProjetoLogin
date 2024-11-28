using Microsoft.AspNetCore.Mvc;

using ProjetoLogin.Models.Usuarios.Enums;
using ProjetoLogin.Models.Usuarios.Filters;
using ProjetoLogin.Models.Usuarios.ViewModels;
using ProjetoLogin.Services.Interfaces;
using ProjetoLogin.Utils.Attributes;

using System.Net;

namespace ProjetoLogin.Controllers
{
	[ApiController]
	[Authorize(TipoUsuario.Gerente)]
	[Route("api/[controller]")]
	public class UsuariosController : ControllerBase
	{
		private readonly IUsuariosService _service;

		public UsuariosController(IUsuariosService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] UsuarioFilter filter)
		{
			return Ok(await _service.GetAllAsync(filter));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			return Ok(await _service.GetByIdAsync(id));
		}

		[AllowAnonymous]
		[HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate(AuthenticateRequest model)
		{
			var response = await _service.GenerateJwtToken(model);

			if (string.IsNullOrWhiteSpace(response))
				return BadRequest(new { message = "Usuário ou Senha inválidos" });

			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] PostUsuario usuario)
		{

			var result = await _service.CreateAsync(usuario);

			if (result == null)
				return Conflict("Usuário já cadastrado");

			if (result == false)
				return BadRequest("Erro ao criar usuário");

			return StatusCode((int)HttpStatusCode.Created);

		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PutUsuario usuario)
		{
			var result = await _service.UpdateAsync(usuario);

			if (result == null)
				return NotFound("Usuário não encontrado");

			if (result == false)
				return BadRequest("Erro ao alterar usuário");

			return Ok(usuario);
		}

		[HttpPut("{id}/status")]
		public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Status status)
		{
			var result = await _service.UpdateStatusAsync(id, status);

			if (result == null)
				return NotFound("Usuário/Status não encontrado");

			if (result == false)
				return BadRequest("Erro ao alterar status do usuário");

			return Ok(status);
		}

		[HttpDelete]
		[Authorize(TipoUsuario.Administrador)]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var result = await _service.DeleteAsync(id);
			if (result == null)
				return NotFound("Usuário não encontrado");
			if (result == false)
				return BadRequest("Erro ao deletar usuário");
			return Ok();
		}
	}
}
