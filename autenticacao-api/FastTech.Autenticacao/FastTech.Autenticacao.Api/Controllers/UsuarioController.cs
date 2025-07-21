using FastTech.Autenticacao.Application.Dtos;
using FastTech.Autenticacao.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.API.Controllers;

[ApiController]
[Route("api/auth")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UsuarioOutputDto>> Login([FromBody] UsuarioLoginDto dto)
    {
        var usuario = await _usuarioService.AutenticarAsync(dto);
        if (usuario == null)
            return Unauthorized();

        return Ok(usuario);
    }

    [HttpPost("cadastro")]
    [AllowAnonymous]
    public async Task<ActionResult<Guid>> Cadastrar([FromBody] UsuarioCadastroDto dto)
    {
        var id = await _usuarioService.CadastrarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id }, id);
    }

    [HttpPatch("{id}/senha")]
    public async Task<IActionResult> AtualizarSenha(Guid id, [FromBody] string novaSenha)
    {
        await _usuarioService.AtualizarSenhaAsync(id, novaSenha);
        return NoContent();
    }

    [HttpPatch("{id}/inativar")]
    public async Task<IActionResult> Inativar(Guid id)
    {
        await _usuarioService.InativarAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/reativar")]
    public async Task<IActionResult> Reativar(Guid id)
    {
        await _usuarioService.ReativarAsync(id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);

        if (usuario is null)
            return NotFound();

        return Ok(usuario);
    }
}