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
    public class ItemQueryRepository : QueryRepositoryBase<Item>, IItemQueryRepository
    {
        public ItemQueryRepository(CatalogoQueryDbContext context) : base(context) { }

        public override async Task<IEnumerable<Item>> ListarTodosAsync()
            => await _querySet
                    .AsNoTracking()
                    .ToListAsync();

        public override async Task<Item?> ObterPorIdAsync(Guid id)
            => await _querySet
                    .FirstOrDefaultAsync(e => e.Id == id && e.DataExclusao == null);

        public async Task<IEnumerable<Item>> ListarPorTipoAsync(Guid idTipoRefeicao)
        {
            return await _querySet
                .AsNoTracking()
                .Where(i => i.TipoRefeicaoId == idTipoRefeicao && i.DataExclusao == null)
                .ToListAsync();
        }

        public async Task<Item?> ObterPorNomeAsync(string nome)
        {
            return await _querySet
                .FirstOrDefaultAsync(i => i.Nome.ToLower().Equals(nome.ToLower()) && i.DataExclusao == null);
        }
    }
}
