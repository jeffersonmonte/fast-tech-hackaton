using FastTech.Kitchen.Domain.Entities;

namespace FastTech.Kitchen.Application.Interfaces
{
    public interface IKitchenService
    {
        Task<IEnumerable<Pedido>> ListarPedidos();
        Task AceitarPedido(Guid id);
        Task RecusarPedido(Guid id, string motivo);
    }
}
