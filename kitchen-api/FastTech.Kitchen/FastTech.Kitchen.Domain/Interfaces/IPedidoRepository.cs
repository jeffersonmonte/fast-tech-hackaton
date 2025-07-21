using FastTech.Kitchen.Domain.Entities;

namespace FastTech.Kitchen.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task Adicionar(Pedido pedido);
        Task<Pedido> ObterPorId(Guid id);
        Task<IEnumerable<Pedido>> ObterTodos();
        Task Atualizar(Pedido pedido);
    }
}
