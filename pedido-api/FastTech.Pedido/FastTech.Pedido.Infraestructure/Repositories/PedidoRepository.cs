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
    public class PedidoRepository : RepositoryBase<Domain.Entities.Pedido>, IPedidoRepository
    {
        public PedidoRepository(PedidoCommandDbContext commandContext, PedidoQueryDbContext queryContext) : base(commandContext, queryContext)
        {
        }

        public async Task<Domain.Entities.Pedido?> ObterPorId(Guid id)
        {
            return await _querySet
                .Include(p => p.Itens)
                .Include(p => p.Historico)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
