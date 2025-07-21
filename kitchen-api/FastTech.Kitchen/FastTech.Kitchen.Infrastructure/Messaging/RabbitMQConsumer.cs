using FastTech.Kitchen.Domain.Entities;
using FastTech.Kitchen.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FastTech.Kitchen.Infrastructure.Messaging
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IPedidoRepository _pedidoRepository;

        public RabbitMQConsumer(IPedidoRepository pedidoRepository)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Mudar para configuração
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _pedidoRepository = pedidoRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var pedido = JsonSerializer.Deserialize<Pedido>(content);

                _pedidoRepository.Adicionar(pedido);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("order.created", false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
