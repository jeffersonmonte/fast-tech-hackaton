using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Infrastructure.Persistence.Command;
using FastTech.Catalogo.Infrastructure.Persistence.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Infrastructure.Repositories;

public class ItemRepository : RepositoryBase<Item>, IItemRepository
{
    public ItemRepository(
        CatalogoCommandDbContext commandContext,
        CatalogoQueryDbContext queryContext)
        : base(commandContext, queryContext) { }

    public async Task<Item?> ObterPorIdAsync(Guid id)
        => await _querySet
            .Include(i => i.TipoRefeicao)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id && i.DataExclusao == null);

    public override async Task<IEnumerable<Item>> ListarTodosAsync()
        => await _querySet
            .Include(i => i.TipoRefeicao)
            .AsNoTracking()
            .Where(i => i.DataExclusao == null)
            .ToListAsync();

    public async Task<IEnumerable<Item>> ListarPorTipoAsync(Guid tipoRefeicaoId)
        => await _querySet
            .Include(i => i.TipoRefeicao)
            .Where(i => i.TipoRefeicaoId == tipoRefeicaoId && i.DataExclusao == null)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Item?> ObterPorNomeAsync(string nome)
        => await _querySet.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Nome.ToLower().Contains(nome.ToLower()) && t.DataExclusao == null);
}