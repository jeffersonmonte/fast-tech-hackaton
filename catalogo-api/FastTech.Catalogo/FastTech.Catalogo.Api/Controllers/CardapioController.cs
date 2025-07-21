using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastTech.Catalogo.API.Controllers
{
    [ApiController]
    [Route("api/cardapio")]
    public class CardapioController : ControllerBase
    {
        private readonly ICardapioService _cardapioService;

        public CardapioController(ICardapioService cardapioService)
        {
            _cardapioService = cardapioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var cardapios = await _cardapioService.ObterTodosAsync();
            return Ok(cardapios);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var cardapio = await _cardapioService.ObterPorIdAsync(id);
            if (cardapio is null)
                return NotFound("Cardápio não encontrado.");

            return Ok(cardapio);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CardapioInputDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _cardapioService.AdicionarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id }, null);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CardapioUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("O ID do cardápio não corresponde ao ID fornecido.");

            await _cardapioService.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            await _cardapioService.RemoverAsync(id);
            return NoContent();
        }
    }
}
