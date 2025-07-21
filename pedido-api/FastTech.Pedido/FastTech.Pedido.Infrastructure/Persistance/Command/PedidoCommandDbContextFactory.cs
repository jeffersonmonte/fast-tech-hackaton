using FastTech.Pedido.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infrastructure.Persistance.Command
{
    public class PedidoCommandDbContextFactory : IDesignTimeDbContextFactory<PedidoCommandDbContext>
    {
        public PedidoCommandDbContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                ?? "localhost";

            var optionsBuilder = new DbContextOptionsBuilder<PedidoCommandDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new PedidoCommandDbContext(optionsBuilder.Options);
        }
    }
}
