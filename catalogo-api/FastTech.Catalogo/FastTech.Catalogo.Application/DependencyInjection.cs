using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ITipoRefeicaoService, TipoRefeicaoService>();
            services.AddScoped<ICardapioService, CardapioService>();

            return services;
        }
    }
}
