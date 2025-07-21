using FastTech.Pedido.Application.Interfaces;
using FastTech.Pedido.Domain.Interfaces.Command;
using FastTech.Pedido.Domain.Interfaces.Query;
using FastTech.Pedido.Infrastructure.Messaging.Publisher;
using FastTech.Pedido.Infrastructure.Persistance.Command;
using FastTech.Pedido.Infrastructure.Persistance.Query;
using FastTech.Pedido.Infrastructure.Repositories.Command;
using FastTech.Pedido.Infrastructure.Repositories.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                ?? throw new InvalidOperationException("Nenhuma string de conexão encontrada para serviço de catalogo.");

            services.AddDbContext<PedidoCommandDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddDbContext<PedidoQueryDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<IPedidoQueryRepository, PedidoQueryRepository>();
            services.AddScoped<IPedidoCommandRepository, PedidoCommandRepository>();

            services.AddScoped<IItemPedidoQueryRepository, ItemPedidoQueryRepository>();
            services.AddScoped<IItemPedidoCommandRepository, ItemPedidoCommandRepository>();

            services.AddScoped<IStatusPedidoHistoricoQueryRepository, StatusPedidoHistoricoQueryRepository>();
            services.AddScoped<IStatusPedidoHistoricoCommandRepository, StatusPedidoHistoricoCommandRepository>();

            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

            return services;
        }

        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("Funcionario", policy =>
                    policy.RequireClaim("Perfil", "Funcionario"))
                .AddPolicy("Gerente", policy =>
                    policy.RequireClaim("Perfil", "Gerente"))
                .AddPolicy("Cliente", policy =>
                    policy.RequireClaim("Perfil", "Cliente"));

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, byte[] key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "application/json";

                        var result = new
                        {
                            message = "Você não tem autorização para acessar este recurso."
                        };

                        var json = JsonConvert.SerializeObject(result);
                        await context.Response.WriteAsync(json);
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";

                        var result = new
                        {
                            message = "Você não tem permissão para acessar este recurso."
                        };

                        var json = JsonConvert.SerializeObject(result);
                        await context.Response.WriteAsync(json);
                    }
                };
            });

            return services;
        }
    }
}
