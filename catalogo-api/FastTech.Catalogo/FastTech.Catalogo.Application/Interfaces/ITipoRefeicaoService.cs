using FastTech.Catalogo.Application.Dtos;

namespace FastTech.Catalogo.Application.Interfaces
{
    public interface ITipoRefeicaoService
    {
        Task<IEnumerable<TipoRefeicaoOutputDto>> ListarTodosAsync();
        Task<TipoRefeicaoOutputDto?> ObterPorIdAsync(Guid id);
        Task<TipoRefeicaoOutputDto?> ObterPorNomeAsync(string nome);
    }
}
