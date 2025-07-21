using FastTech.Autenticacao.Application.Interfaces;
using FastTech.Autenticacao.Domain.Interfaces;
using FastTech.Autenticacao.Infrastructure.Persistance.Command;
using FastTech.Autenticacao.Infrastructure.Persistance.Query;
using FastTech.Autenticacao.Infrastructure.Repositories;
using FastTech.Autenticacao.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                ?? throw new InvalidOperationException("Nenhuma string de conexão encontrada para serviço de catalogo.");

            services.AddDbContext<AutenticacaoCommandDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddDbContext<AutenticacaoQueryDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }
    }
}
