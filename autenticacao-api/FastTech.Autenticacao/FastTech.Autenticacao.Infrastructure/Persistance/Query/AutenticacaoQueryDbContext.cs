using FastTech.Autenticacao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Infrastructure.Persistance.Query
{
    public class AutenticacaoQueryDbContext : DbContext
    {
        public AutenticacaoQueryDbContext(DbContextOptions<AutenticacaoQueryDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutenticacaoQueryDbContext).Assembly);
        }
    }
}
