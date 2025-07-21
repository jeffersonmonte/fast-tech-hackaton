using System;
using System.ComponentModel;
using System.Linq.Expressions;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Infrastructure.Persistence.Command;
using FastTech.Catalogo.Infrastructure.Persistence.Query;
using Microsoft.EntityFrameworkCore;

namespace FastTech.Catalogo.Infrastructure.Repositories;

public class CardapioRepository : RepositoryBase<Cardapio>, ICardapioRepository
{
    public CardapioRepository(CatalogoCommandDbContext commandContext, CatalogoQueryDbContext queryContext)
    : base(commandContext, queryContext) { }

    public override async Task<IEnumerable<Cardapio>> ListarTodosAsync()
    => await _querySet.AsNoTracking()
        .Include(c => c.Itens)
        .ThenInclude(i => i.TipoRefeicao)
        .Where(t => t.DataExclusao == null)
        .ToListAsync();

    public async Task<Cardapio?> ObterPorNomeAsync(string nome)
    => await _querySet.AsNoTracking()
        .Include(c => c.Itens)
        .ThenInclude(i => i.TipoRefeicao)
        .FirstOrDefaultAsync(t => t.Nome.ToLower().Contains(nome.ToLower()) && t.DataExclusao == null);

    public async Task<Cardapio?> ObterPorIdAsync(Guid id)
    => await _querySet
        .AsNoTracking()
        .Include(c => c.Itens)
        .ThenInclude(i => i.TipoRefeicao)
        .FirstOrDefaultAsync(i => i.Id == id && i.DataExclusao == null);

    public override async Task AdicionarAsync(Cardapio entidade)
    {
        AtualizarItens(entidade);
        await _commandSet.AddAsync(entidade);
    }

    public override void Atualizar(Cardapio entidade)
    {
        var entidade_bd = _commandSet.FirstOrDefault(c => c.Id == entidade.Id);

        if (entidade_bd is null)
            return;

        entidade_bd.AssociarIdItens(entidade.ItensPendentes);
        entidade_bd.Atualizar(entidade.Nome, entidade.Descricao);

        AtualizarItens(entidade_bd);

        _commandSet.Update(entidade_bd);
    }

    public override void Remover(Cardapio entidade)
    {
        var entidade_bd = _commandSet.FirstOrDefault(c => c.Id == entidade.Id);

        if (entidade_bd is null)
            return;

        entidade_bd.Excluir();

        _commandContext.Update(entidade_bd);
    }

    private void AtualizarItens(Cardapio entidade)
    {
        foreach (var idItem in entidade.ItensPendentes)
        {
            var item = _commandContext.Itens.FirstOrDefault(i => i.Id == idItem);

            if (item is not null)
                entidade.AdicionarItem(item);
        }
        entidade.LimparIdItensAssociados();
    }
}
