using FastTech.Pedido.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infraestructure.Persistance.Command
{
    public class PedidoCommandDbContext : DbContext
    {
        public PedidoCommandDbContext(DbContextOptions<PedidoCommandDbContext> options)
            : base(options) { }

        public DbSet<Domain.Entities.Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> Itens { get; set; }
        public DbSet<StatusPedidoHistorico> StatusPedidoHistoricos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidoCommandDbContext).Assembly);
        }
    }
}
