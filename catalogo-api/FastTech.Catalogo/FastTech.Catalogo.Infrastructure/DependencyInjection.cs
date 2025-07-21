using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Domain.Interfaces.Query;
using FastTech.Catalogo.Infrastructure.Messaging.Publisher;
using FastTech.Catalogo.Infrastructure.Persistence.Command;
using FastTech.Catalogo.Infrastructure.Persistence.Query;
using FastTech.Catalogo.Infrastructure.Repositories;
using FastTech.Catalogo.Infrastructure.Repositories.Command;
using FastTech.Catalogo.Infrastructure.Repositories.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                ?? throw new InvalidOperationException("Nenhuma string de conexão encontrada para serviço de catalogo.");

            services.AddDbContext<CatalogoCommandDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddDbContext<CatalogoQueryDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<ITipoRefeicaoCommandRepository, TipoRefeicaoCommandRepository>();
            services.AddScoped<ITipoRefeicaoQueryRepository, TipoRefeicaoQueryRepository>();

            services.AddScoped<IItemCommandRepository, ItemCommandRepository>();
            services.AddScoped<IItemQueryRepository, ItemQueryRepository>();

            services.AddScoped<ICardapioCommandRepository, CardapioCommandRepository>();
            services.AddScoped<ICardapioQueryRepository, CardapioQueryRepository>();

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
