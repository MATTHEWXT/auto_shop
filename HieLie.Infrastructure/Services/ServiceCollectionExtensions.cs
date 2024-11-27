using HieLie.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using StackExchange.Redis;


namespace HieLie.Infrastructure.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString)
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            services.AddTransient<ICacheService, RedisCacheService>();

            return services;
        }

        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(sp => new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            });

            services.AddScoped<IRabbitMqService, RabbitMqService>();

            return services;
        }
    }
}
