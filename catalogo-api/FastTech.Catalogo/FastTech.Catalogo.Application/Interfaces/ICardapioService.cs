using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;

namespace FastTech.Catalogo.Application.Interfaces
{
    public interface ICardapioService
    {
        Task<IEnumerable<CardapioOutputDto>> ObterTodosAsync();
        Task<CardapioOutputDto?> ObterPorIdAsync(Guid id);
        Task<Guid> AdicionarAsync(CardapioInputDto cardapio);
        Task AtualizarAsync(CardapioUpdateDto cardapio);
        Task RemoverAsync(Guid id);
    }
}
