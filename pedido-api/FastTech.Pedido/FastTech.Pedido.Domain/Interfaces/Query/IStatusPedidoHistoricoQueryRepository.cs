using FastTech.Pedido.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.Interfaces.Query
{
    public interface IStatusPedidoHistoricoQueryRepository : IQueryRepository<StatusPedidoHistorico>
    {
        Task<IEnumerable<StatusPedidoHistorico>> ListarPorPedidoAsync(Guid pedidoId);
    }
}
