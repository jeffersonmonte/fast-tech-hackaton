using FastTech.Catalogo.Domain.Entities;

namespace FastTech.Catalogo.Domain.Interfaces
{
    public interface IItemRepository : IRepositoryBase<Item>
    {
        Task<Item?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Item>> ListarPorTipoAsync(Guid tipoRefeicaoId);
        Task<Item?> ObterPorNomeAsync(string nome);
    }
}
