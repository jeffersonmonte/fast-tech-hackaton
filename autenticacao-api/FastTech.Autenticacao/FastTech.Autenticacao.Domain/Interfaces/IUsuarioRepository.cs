using FastTech.Autenticacao.Domain.Entities;

namespace FastTech.Autenticacao.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<Usuario?> ObterPorIdAsync(Guid id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<Usuario?> ObterPorCpfAsync(string cpf);
        Task<bool> ExisteComEmailAsync(string email);
        Task<bool> ExisteComCpfAsync(string cpf);
    }
}
