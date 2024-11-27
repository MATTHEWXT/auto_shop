using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace HieLie.Infrastructure.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnectionFactory _connectionFactory;

        public RabbitMqService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task Publish(string message)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "my_exchange",
                type: "direct",
                durable: true
                );

            await channel.QueueDeclareAsync(queue: "task_queue",
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

            await channel.QueueBindAsync(
                queue: "task_queue",
                exchange: "my_exchange",
                routingKey: "my_key");

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: "my_exchange",
                                            routingKey: "my_key",
                                            body: body);

            Console.WriteLine($"[x] Sent {message}");
        }

        public async Task Consume()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "task_queue",
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);


            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");

                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue: "task_queue",
                                      autoAck: true,
                                      consumer: consumer);

            Console.WriteLine(" [*] Waiting for messages...");
        }
    }
}
