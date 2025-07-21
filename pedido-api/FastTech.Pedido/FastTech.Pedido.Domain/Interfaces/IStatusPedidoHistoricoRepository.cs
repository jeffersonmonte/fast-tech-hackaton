using FastTech.Pedido.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.Interfaces
{
    public interface IStatusPedidoHistoricoRepository : IRepositoryBase<StatusPedidoHistorico>
    {
        Task<IEnumerable<StatusPedidoHistorico>> ListarPorPedidoAsync(Guid pedidoId);
    }
}
