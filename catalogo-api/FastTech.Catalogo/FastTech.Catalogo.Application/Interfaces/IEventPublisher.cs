namespace FastTech.Catalogo.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(string exchangeName, string queueName, T message, CancellationToken cancellationToken = default);
    }
}
