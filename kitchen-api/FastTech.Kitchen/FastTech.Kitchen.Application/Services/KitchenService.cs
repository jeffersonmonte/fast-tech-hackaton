using FastTech.Kitchen.Application.Interfaces;
using FastTech.Kitchen.Domain.Entities;
using FastTech.Kitchen.Domain.Interfaces;

namespace FastTech.Kitchen.Application.Services
{
    public class KitchenService : IKitchenService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public KitchenService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<IEnumerable<Pedido>> ListarPedidos()
        {
            return await _pedidoRepository.ObterTodos();
        }

        public async Task AceitarPedido(Guid id)
        {
            var pedido = await _pedidoRepository.ObterPorId(id);
            if (pedido != null)
            {
                pedido.Aceitar();
                await _pedidoRepository.Atualizar(pedido);
            }
        }

        public async Task RecusarPedido(Guid id, string motivo)
        {
            var pedido = await _pedidoRepository.ObterPorId(id);
            if (pedido != null)
            {
                pedido.Recusar(motivo);
                await _pedidoRepository.Atualizar(pedido);
            }
        }
    }
}
