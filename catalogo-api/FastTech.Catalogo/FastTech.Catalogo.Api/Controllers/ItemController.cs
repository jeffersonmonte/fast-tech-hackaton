using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTech.Catalogo.Api.Controllers
{
    [ApiController]
    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemOutputDto>>> GetTodos()
        {
            var itens = await _itemService.ListarTodosAsync();
            return Ok(itens);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ItemOutputDto>> GetPorId(Guid id)
        {
            var item = await _itemService.ObterPorIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("tipo/{tipoId:int}")]
        public async Task<ActionResult<IEnumerable<ItemOutputDto>>> GetPorTipo(Guid tipoId)
        {
            var itens = await _itemService.ListarPorTipoAsync(tipoId);
            return Ok(itens);
        }

        [Authorize(Policy = "Gerente")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ItemInputDto item)
        {
            var id = await _itemService.AdicionarAsync(item);
            return CreatedAtAction(nameof(GetPorId), new { id }, null);
        }

        [Authorize(Policy = "Gerente")]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult> Put([FromBody] ItemUpdateDto item)
        {
            await _itemService.AtualizarAsync(item);
            return NoContent();
        }

        [Authorize(Policy = "Gerente")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _itemService.RemoverAsync(id);
            return NoContent();
        }
    }
}
