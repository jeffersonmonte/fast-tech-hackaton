using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Query;
using FastTech.Catalogo.Infrastructure.Persistence.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Infrastructure.Repositories.Query
{
    public class CardapioQueryRepository : QueryRepositoryBase<Cardapio>, ICardapioQueryRepository
    {
        public CardapioQueryRepository(CatalogoQueryDbContext context) : base(context) { }

        public override async Task<IEnumerable<Cardapio>> ListarTodosAsync()
            => await _querySet
                    .Include(c => c.Itens)
                    .AsNoTracking()
                    .ToListAsync();

        public override async Task<Cardapio?> ObterPorIdAsync(Guid id)
            => await _querySet
                    .Include(c => c.Itens)
                    .FirstOrDefaultAsync(e => e.Id == id && e.DataExclusao == null);
    }
}
