using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FastTech.Pedido.Infraestructure.Messaging
{
    public class RabbitMQProducer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQProducer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Mudar para configuração
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publicar<T>(string exchange, string routingKey, T data)
        {
            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);

            var message = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: exchange,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
