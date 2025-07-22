using FastTech.Kitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FastTech.Kitchen.Infrastructure.Persistence.Command
{
    public class KitchenDbContext : DbContext
    {
        public KitchenDbContext(DbContextOptions<KitchenDbContext> options) : base(options)
        {
        }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
