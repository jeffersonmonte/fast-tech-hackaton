using FastTech.Autenticacao.Domain.Entities;
using FastTech.Autenticacao.Domain.Interfaces;
using FastTech.Autenticacao.Infrastructure.Persistance.Command;
using FastTech.Autenticacao.Infrastructure.Persistance.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Infrastructure.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AutenticacaoCommandDbContext commandContext, AutenticacaoQueryDbContext queryContext)
            : base(commandContext, queryContext)
        {
        }

        public async Task<Usuario?> ObterPorIdAsync(Guid id)
        {
            return await _querySet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _querySet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Endereco.ToLower().Equals(email.ToLower()));
        }

        public async Task<Usuario?> ObterPorCpfAsync(string cpf)
        {
            return await _querySet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Cpf != null && u.Cpf.Numero == cpf);
        }

        public async Task<bool> ExisteComEmailAsync(string email)
        {
            return await _querySet
                .AsNoTracking()
                .AnyAsync(u => u.Email.Endereco.ToLower().Equals(email.ToLower()));
        }

        public async Task<bool> ExisteComCpfAsync(string cpf)
        {
            return await _querySet
                .AsNoTracking()
                .AnyAsync(u => u.Cpf != null && u.Cpf.Numero == cpf);
        }
    }
}


