namespace HieLie.Infrastructure.Services
{
    public interface IRabbitMqService
    {
        Task Consume();
        Task Publish(string message);
    }
}