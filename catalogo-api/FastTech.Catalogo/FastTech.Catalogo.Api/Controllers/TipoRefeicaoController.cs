using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTech.Catalogo.Api.Controllers
{
    [ApiController]
    [Route("api/tipo-refeicao")]
    public class TipoRefeicaoController : ControllerBase
    {
        private readonly ITipoRefeicaoService _tipoService;

        public TipoRefeicaoController(ITipoRefeicaoService tipoService)
        {
            _tipoService = tipoService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoRefeicao>>> GetTodos()
        {
            var tipos = await _tipoService.ListarTodosAsync();
            return Ok(tipos);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TipoRefeicao>> GetPorId(Guid id)
        {
            var tipo = await _tipoService.ObterPorIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [AllowAnonymous]
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<TipoRefeicao>> GetPorNome(string nome)
        {
            var tipo = await _tipoService.ObterPorNomeAsync(nome);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }
    }
}
