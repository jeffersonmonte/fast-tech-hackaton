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
    public class TipoRefeicaoQueryRepository : QueryRepositoryBase<TipoRefeicao>, ITipoRefeicaoQueryRepository
    {
        public TipoRefeicaoQueryRepository(CatalogoQueryDbContext context) : base(context) { }

        public override async Task<IEnumerable<TipoRefeicao>> ListarTodosAsync()
            => await _querySet
                    .AsNoTracking()
                    .ToListAsync();

        public override async Task<TipoRefeicao?> ObterPorIdAsync(Guid id)
            => await _querySet
                    .FirstOrDefaultAsync(t => t.Id == id && t.DataExclusao == null);
        public async Task<TipoRefeicao?> ObterPorNomeAsync(string nome)
            => await _querySet
                .FirstOrDefaultAsync(t => t.Nome.ToLower().Equals(nome.ToLower()) && t.DataExclusao == null);
    }
}
