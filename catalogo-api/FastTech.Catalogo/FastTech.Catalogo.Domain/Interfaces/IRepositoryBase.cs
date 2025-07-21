using FastTech.Catalogo.Domain.Entities;
using System.Linq.Expressions;

namespace FastTech.Catalogo.Domain.Interfaces
{
    public interface IRepositoryBase<T> where T : EntidadeBase
    {
        Task<IEnumerable<T>> ListarTodosAsync();
        Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>>? filtro = null);
        Task<T?> ObterAsync(Expression<Func<T, bool>> filtro);
        Task<bool> ExisteAsync(Expression<Func<T, bool>> filtro);
        Task AdicionarAsync(T entidade);
        void Atualizar(T entidade);
        void Remover(T entidade);
        Task SalvarAlteracoesAsync();
    }
}
