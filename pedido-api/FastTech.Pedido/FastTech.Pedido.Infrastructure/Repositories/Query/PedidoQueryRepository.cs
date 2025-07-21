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
using System.Linq.Expressions;
using System.Text;

namespace FastTech.Pedido.Infrastructure.Repositories.Query
{
    public class PedidoQueryRepository : QueryRepositoryBase<Domain.Entities.Pedido>, IPedidoQueryRepository
    {
        public PedidoQueryRepository(PedidoQueryDbContext context) : base(context) { }

        public override async Task<Domain.Entities.Pedido?> ObterPorIdAsync(Guid id)
            => await _querySet
                .Include(p => p.Itens)
                .Include(p => p.Historico)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.DataHoraCancelamento == null);

        public override async Task<IEnumerable<Domain.Entities.Pedido>> ListarAsync(Expression<Func<Domain.Entities.Pedido, bool>>? filtro = null)
        {
            var query = _querySet.AsNoTracking();

            if (filtro != null)
                query = query.Where(filtro);

            return await query
                .Include(p => p.Itens)
                .Include(p => p.Historico)
                .ToListAsync();
        }
    }
}
