using Microsoft.AspNetCore.Mvc;

using ProjetoLogin.Models.Enums;
using ProjetoLogin.Services.Interfaces;

using System.Net;
using ProjetoLogin.Attributes;
using ProjetoLogin.Models.ViewModels.Usuarios;

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
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostUsuario usuario)
        {
            try
            {
                var result = await _service.CreateAsync(usuario);

                if (result == null)
                    return Conflict("Usuário já cadastrado");

                if (result == false)
                    return BadRequest("Erro ao criar usuário");
                
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return BadRequest("Erro ao criar usuário");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PutUsuario usuario)
        {
            try
            {
                var result = await _service.UpdateAsync(usuario);

                if (result == null)
                    return NotFound("Usuário não encontrado");

                if (result == false)
                    return BadRequest("Erro ao alterar usuário");

                return Ok(usuario);
            }
            catch (Exception e)
            {
                return BadRequest("Erro ao alterar usuário");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Status status)
        {
            try
            {
                var result = await _service.UpdateStatusAsync(id, status);

                if (result == null)
                    return NotFound("Usuário/Status não encontrado");

                if (result == false)
                    return BadRequest("Erro ao alterar status do usuário");
                
                return Ok(status);
            }
            catch (Exception e)
            {
                return BadRequest("Erro ao alterar status usuário");
            }
        }

        [HttpGet]
        [Authorize(TipoUsuario.Administrador)]
        public async Task<IActionResult> GetAll([FromQuery] Status? status)
        {
            try
            {
                return Ok(await _service.GetAllAsync(status));
            }
            catch (Exception e)
            {
                return BadRequest("Erro ao listar usuários");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                return Ok(await _service.GetByIdAsync(id));
            }
            catch (Exception e)
            {
                return BadRequest("Erro ao obter usuário");
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _service.GenerateJwtToken(model);

            if (string.IsNullOrWhiteSpace(response))
                return BadRequest(new { message = "Username or password is invalid" });

            return Ok(response);
        }

    }
}
