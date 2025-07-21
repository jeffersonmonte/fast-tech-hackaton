using FastTech.Pedido.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infrastructure.Persistance.Query
{
    public class PedidoQueryDbContext : DbContext
    {
        public PedidoQueryDbContext(DbContextOptions<PedidoQueryDbContext> options)
            : base(options) { }

        public DbSet<Domain.Entities.Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> Itens { get; set; }
        public DbSet<StatusPedidoHistorico> StatusPedidoHistoricos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidoQueryDbContext).Assembly);
        }
    }
}
