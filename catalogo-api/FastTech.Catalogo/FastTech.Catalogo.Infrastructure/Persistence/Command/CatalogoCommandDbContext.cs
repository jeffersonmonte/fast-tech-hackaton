using FastTech.Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Infrastructure.Persistence.Command
{
    public class CatalogoCommandDbContext : DbContext
    {
        public CatalogoCommandDbContext(DbContextOptions<CatalogoCommandDbContext> options)
            : base(options) { }

        public DbSet<Cardapio> Cardapios { get; set; }
        public DbSet<Item> Itens { get; set; }
        public DbSet<TipoRefeicao> TiposRefeicao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoCommandDbContext).Assembly);
        }

        public async Task SeedAsync()
        {
            IEnumerable<string> refeicoesIniciais = ["Lanche", "Bebida"];
            foreach(var refeicao in refeicoesIniciais)
            {
                if (TiposRefeicao.Any(t => t.Nome.ToLower().Equals(refeicao.ToLower())))
                    continue;

                await TiposRefeicao.AddAsync(new TipoRefeicao(refeicao));
            }
            await SaveChangesAsync();
        }
    }
}
