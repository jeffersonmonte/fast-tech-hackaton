using FastTech.Catalogo.Application.Dtos;

namespace FastTech.Catalogo.Application.Interfaces
{
    public interface IItemService
    {
        Task<ItemOutputDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ItemOutputDto>> ListarTodosAsync();
        Task<IEnumerable<ItemOutputDto>> ListarPorTipoAsync(Guid tipoRefeicaoId);
        Task<Guid> AdicionarAsync(ItemInputDto dto);
        Task AtualizarAsync(ItemUpdateDto dto);
        Task RemoverAsync(Guid id);
    }
}
