using FastTech.Autenticacao.Application.Interfaces;
using FastTech.Autenticacao.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();

            return services;
        }
    }
}
