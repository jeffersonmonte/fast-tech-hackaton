using FastTech.Pedido.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.Interfaces
{
    public interface IQueryRepository<T> where T : EntidadeBase
    {
        Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>>? filtro = null);
        Task<T?> ObterAsync(Expression<Func<T, bool>> filtro);
        Task<bool> ExisteAsync(Expression<Func<T, bool>> filtro);
        Task<IEnumerable<T>> ListarTodosAsync();
        Task<T?> ObterPorIdAsync(Guid id);
    }

    public interface ICommandRepository<T> where T : EntidadeBase
    {
        Task AdicionarAsync(T entidade);
        Task<T?> ObterPorIdAsync(Guid id);
        void Remover(T entidade);
        Task SalvarAlteracoesAsync();
    }
}
