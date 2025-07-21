using FastTech.Autenticacao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Infrastructure.Persistance.Command
{
    public class AutenticacaoCommandDbContext : DbContext
    {
        public AutenticacaoCommandDbContext(DbContextOptions<AutenticacaoCommandDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutenticacaoCommandDbContext).Assembly);
        }
    }
}
