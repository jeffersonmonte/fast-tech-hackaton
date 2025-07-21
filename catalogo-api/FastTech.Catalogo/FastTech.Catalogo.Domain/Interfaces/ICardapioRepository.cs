using System;
using FastTech.Catalogo.Domain.Entities;

namespace FastTech.Catalogo.Domain.Interfaces;

public interface ICardapioRepository : IRepositoryBase<Cardapio>
{
    Task<Cardapio?> ObterPorIdAsync(Guid id);
    Task<Cardapio?> ObterPorNomeAsync(string nome);
}
