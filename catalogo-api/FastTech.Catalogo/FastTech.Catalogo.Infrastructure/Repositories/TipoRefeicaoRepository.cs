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

public class TipoRefeicaoRepository : RepositoryBase<TipoRefeicao>, ITipoRefeicaoRepository
{
    public TipoRefeicaoRepository(
        CatalogoCommandDbContext commandContext,
        CatalogoQueryDbContext queryContext)
        : base(commandContext, queryContext) { }

    public override async Task<IEnumerable<TipoRefeicao>> ListarTodosAsync()
     => await _querySet.AsNoTracking()
        .Where(t => t.DataExclusao == null).ToListAsync();

    public async Task<TipoRefeicao?> ObterPorIdAsync(Guid id)
        => await _querySet.AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id && t.DataExclusao == null);

    public async Task<TipoRefeicao?> ObterPorNomeAsync(string nome)
        => await _querySet.AsNoTracking()
        .FirstOrDefaultAsync(t => t.Nome.ToLower() == nome.ToLower() && t.DataExclusao == null);
}