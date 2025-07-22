using FastTech.Kitchen.Application.Interfaces;
using FastTech.Kitchen.Application.Services;
using FastTech.Kitchen.Domain.Interfaces;
using Prometheus;
using FastTech.Kitchen.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FastTech.Kitchen.Infrastructure.Persistence.Command;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 2) Register repositories and application services
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IKitchenService, KitchenService>();

// 3) Register background RabbitMQ consumer
builder.Services.AddHostedService<FastTech.Kitchen.Infrastructure.Messaging.RabbitMQConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();