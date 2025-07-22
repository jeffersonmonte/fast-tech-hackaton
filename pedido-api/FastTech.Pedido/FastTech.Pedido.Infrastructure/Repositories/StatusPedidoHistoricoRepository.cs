using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Interfaces;
using FastTech.Pedido.Infraestructure.Persistance.Command;
using FastTech.Pedido.Infraestructure.Persistance.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infraestructure.Repositories
{
    public class StatusPedidoHistoricoRepository : RepositoryBase<StatusPedidoHistorico>, IStatusPedidoHistoricoRepository
    {
        public StatusPedidoHistoricoRepository(PedidoCommandDbContext commandContext, PedidoQueryDbContext queryContext) : base(commandContext, queryContext)
        {
        }

        public async Task<IEnumerable<StatusPedidoHistorico>> ListarPorPedidoAsync(Guid pedidoId)
        {
            return await _querySet
                .Where(h => EF.Property<Guid>(h, "PedidoId") == pedidoId)
                .OrderBy(h => h.DataHora)
                .ToListAsync();
        }
    }
}
