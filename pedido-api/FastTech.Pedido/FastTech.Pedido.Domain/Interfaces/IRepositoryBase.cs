using FastTech.Pedido.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.Interfaces
{
    public interface IRepositoryBase<T> where T : EntidadeBase
    {
        Task<IEnumerable<T>> ListarTodosAsync();
        Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>>? filtro = null);
        Task<bool> ExisteAsync(Expression<Func<T, bool>> filtro);
        Task AdicionarAsync(T entidade);
        void Atualizar(T entidade);
        void Remover(T entidade);
        Task SalvarAlteracoesAsync();
    }
}
