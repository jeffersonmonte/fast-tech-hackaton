using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Interfaces.Command;
using FastTech.Pedido.Domain.Interfaces.Query;
using FastTech.Pedido.Infrastructure.Persistance.Command;
using FastTech.Pedido.Infrastructure.Persistance.Query;
using FastTech.Pedido.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastTech.Pedido.Infrastructure.Repositories.Query
{
    public class StatusPedidoHistoricoQueryRepository : QueryRepositoryBase<StatusPedidoHistorico>, IStatusPedidoHistoricoQueryRepository
    {
        public StatusPedidoHistoricoQueryRepository(PedidoQueryDbContext context) : base(context) { }

        public async Task<IEnumerable<StatusPedidoHistorico>> ListarPorPedidoAsync(Guid pedidoId)
            => await _querySet
                .Where(h => EF.Property<Guid>(h, "PedidoId") == pedidoId)
                .OrderBy(h => h.DataHora)
                .ToListAsync();
    }
}
