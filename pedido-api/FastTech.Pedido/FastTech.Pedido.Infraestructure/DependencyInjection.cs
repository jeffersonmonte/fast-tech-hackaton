using FastTech.Pedido.Domain.Interfaces;
using FastTech.Pedido.Infraestructure.Persistance.Command;
using FastTech.Pedido.Infraestructure.Persistance.Query;
using FastTech.Pedido.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PedidoCommandDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddDbContext<PedidoQueryDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();
            services.AddScoped<IStatusPedidoHistoricoRepository, StatusPedidoHistoricoRepository>();

            return services;
        }
    }
}
