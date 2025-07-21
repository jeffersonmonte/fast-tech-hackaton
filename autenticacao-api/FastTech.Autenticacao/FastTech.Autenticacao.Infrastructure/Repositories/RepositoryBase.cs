using FastTech.Autenticacao.Domain.Entities;
using FastTech.Autenticacao.Domain.Interfaces;
using FastTech.Autenticacao.Infrastructure.Persistance.Command;
using FastTech.Autenticacao.Infrastructure.Persistance.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : EntidadeBase
    {
        protected readonly AutenticacaoCommandDbContext _commandContext;
        protected readonly AutenticacaoQueryDbContext _queryContext;
        protected readonly DbSet<T> _commandSet;
        protected readonly DbSet<T> _querySet;

        protected RepositoryBase(AutenticacaoCommandDbContext commandContext, AutenticacaoQueryDbContext queryContext)
        {
            _commandContext = commandContext;
            _queryContext = queryContext;
            _commandSet = _commandContext.Set<T>();
            _querySet = _queryContext.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>>? filtro = null)
        {
            var query = _querySet.AsNoTracking();

            if (filtro != null)
                query = query.Where(filtro);

            return await query.ToListAsync();
        }

        public virtual async Task<T?> ObterAsync(Expression<Func<T, bool>> filtro)
            => await _querySet.AsNoTracking().FirstOrDefaultAsync(filtro);

        public virtual async Task<bool> ExisteAsync(Expression<Func<T, bool>> filtro)
            => await _querySet.AsNoTracking().AnyAsync(filtro);

        public virtual async Task AdicionarAsync(T entidade)
            => await _commandSet.AddAsync(entidade);

        public virtual void Atualizar(T entidade)
            => _commandSet.Update(entidade);

        public virtual void Remover(T entidade)
            => _commandSet.Remove(entidade);

        public virtual async Task SalvarAlteracoesAsync()
            => await _commandContext.SaveChangesAsync();

        public virtual async Task<IEnumerable<T>> ListarTodosAsync()
            => await _querySet.AsNoTracking().ToListAsync();
    }
}
