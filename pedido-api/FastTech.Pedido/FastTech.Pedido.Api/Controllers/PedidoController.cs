using FastTech.Pedido.Application.Dtos;
using FastTech.Pedido.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTech.Pedido.Api.Controllers
{
    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [Authorize(Policy = "Cliente")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CriarPedido([FromBody] PedidoInputDto dto)
        {
            var id = await _pedidoService.CriarPedidoAsync(dto);
            return CreatedAtAction(nameof(ObterPedidoPorId), new { id }, id);
        }

        [Authorize(Policy = "Cliente")]
        [HttpPatch("cancelar")]
        public async Task<IActionResult> CancelarPedido([FromBody] PedidoCancelamentoDto dto)
        {
            await _pedidoService.CancelarPedidoAsync(dto);
            return NoContent();
        }

        [Authorize(Policy = "Funcionario")]
        [HttpPatch("status")]
        public async Task<IActionResult> AtualizarStatusPedido([FromBody] PedidoUpdateStatusDto dto)
        {
            await _pedidoService.AtualizarStatusAsync(dto);
            return NoContent();
        }

        [Authorize(Policy = "Cliente")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoOutputDto>> ObterPedidoPorId(Guid id)
        {
            var pedido = await _pedidoService.ObterPedidoCompletoAsync(id);
            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        [Authorize(Policy = "Cliente")]
        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<IEnumerable<PedidoOutputDto>>> ListarPedidosCliente(Guid idCliente)
        {
            var pedidos = await _pedidoService.ListarPedidosClienteAsync(idCliente);
            return Ok(pedidos);
        }
    }
}
