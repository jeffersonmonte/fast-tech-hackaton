﻿using FastTech.Pedido.Application.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infrastructure.Messaging.Publisher
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqEventPublisher()
        {
            var hostname = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
            _factory = new ConnectionFactory() { HostName = hostname };
        }

        public async Task PublishAsync<T>(string exchangeName, string queueName, T message, CancellationToken cancellationToken = default)
        {
            var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            if (enviroment.Equals("Development", StringComparison.CurrentCultureIgnoreCase))
                return;

            using var connection = await _factory.CreateConnectionAsync(cancellationToken);
            using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(exchange: exchangeName,
                                             routingKey: queueName,
                                             mandatory: false,
                                             properties,
                                             body,
                                             cancellationToken: cancellationToken);
        }
    }
}
