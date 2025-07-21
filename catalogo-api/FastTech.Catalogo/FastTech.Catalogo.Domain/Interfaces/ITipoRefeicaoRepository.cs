using FastTech.Catalogo.Domain.Entities;

namespace FastTech.Catalogo.Domain.Interfaces
{
    public interface ITipoRefeicaoRepository : IRepositoryBase<TipoRefeicao>
    {
        Task<TipoRefeicao?> ObterPorIdAsync(Guid id);
        Task<TipoRefeicao?> ObterPorNomeAsync(string nome);
    }
}
