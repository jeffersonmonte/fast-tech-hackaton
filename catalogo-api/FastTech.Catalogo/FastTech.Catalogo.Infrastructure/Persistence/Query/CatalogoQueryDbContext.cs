using FastTech.Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Infrastructure.Persistence.Query
{
    public class CatalogoQueryDbContext : DbContext
    {
        public CatalogoQueryDbContext(DbContextOptions<CatalogoQueryDbContext> options)
            : base(options) { }

        public DbSet<Item> Itens { get; set; }
        public DbSet<Cardapio> Cardapios { get; set; }
        public DbSet<TipoRefeicao> TiposRefeicao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoQueryDbContext).Assembly);
        }
    }
}
