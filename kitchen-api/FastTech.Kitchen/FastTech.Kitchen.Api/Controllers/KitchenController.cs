using FastTech.Kitchen.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FastTech.Kitchen.Api.Controllers
{
    [ApiController]
    [Route("api/kitchen")]
    public class KitchenController : ControllerBase
    {
        private readonly IKitchenService _kitchenService;

        public KitchenController(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> ListarPedidos()
        {
            var pedidos = await _kitchenService.ListarPedidos();
            return Ok(pedidos);
        }

        [HttpPost("orders/{id}/accept")]
        public async Task<IActionResult> AceitarPedido(Guid id)
        {
            await _kitchenService.AceitarPedido(id);
            return Ok();
        }

        [HttpPost("orders/{id}/reject")]
        public async Task<IActionResult> RecusarPedido(Guid id, [FromBody] string motivo)
        {
            await _kitchenService.RecusarPedido(id, motivo);
            return Ok();
        }
    }
}
