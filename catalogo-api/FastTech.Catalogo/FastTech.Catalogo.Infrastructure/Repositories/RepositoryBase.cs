using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Infrastructure.Persistence.Command;
using FastTech.Catalogo.Infrastructure.Persistence.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Infrastructure.Repositories
{
    public abstract class QueryRepositoryBase<T> : IQueryRepository<T> where T : EntidadeBase
    {
        protected readonly CatalogoQueryDbContext _queryContext;
        protected readonly DbSet<T> _querySet;

        protected QueryRepositoryBase(CatalogoQueryDbContext queryContext)
        {
            _queryContext = queryContext;
            _querySet = _queryContext.Set<T>();
        }

        public virtual async Task<T?> ObterPorIdAsync(Guid id)
            => await _querySet.FirstOrDefaultAsync(e => e.Id == id);

        public virtual async Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>>? filtro = null)
        {
            var query = _querySet.AsNoTracking();

            if (filtro != null)
                query = query.Where(filtro);

            return await query.ToListAsync();
        }

        public virtual async Task<T?> ObterAsync(Expression<Func<T, bool>> filtro)
            => await _querySet.AsNoTracking().FirstOrDefaultAsync(filtro);

        public virtual async Task<IEnumerable<T>> ListarTodosAsync()
            => await _querySet.AsNoTracking().ToListAsync();

        public virtual async Task<bool> ExisteAsync(Expression<Func<T, bool>> filtro)
            => await _querySet.AsNoTracking().AnyAsync(filtro);
    }

    public abstract class CommandRepositoryBase<T> : ICommandRepository<T> where T : EntidadeBase
    {
        protected readonly CatalogoCommandDbContext _commandContext;
        protected readonly DbSet<T> _commandSet;

        protected CommandRepositoryBase(CatalogoCommandDbContext commandContext)
        {
            _commandContext = commandContext;
            _commandSet = _commandContext.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>>? filtro = null)
        {
            if (filtro != null)
                return await _commandSet.Where(filtro).ToListAsync();

            return await _commandSet.ToListAsync();
        }

        public virtual async Task AdicionarAsync(T entidade)
            => await _commandSet.AddAsync(entidade);

        public virtual void Remover(T entidade)
            => _commandSet.Remove(entidade);

        public virtual async Task SalvarAlteracoesAsync()
            => await _commandContext.SaveChangesAsync();

        public virtual async Task<T?> ObterPorIdAsync(Guid id)
            => await _commandSet.FirstOrDefaultAsync(e => e.Id == id);
    }
}
