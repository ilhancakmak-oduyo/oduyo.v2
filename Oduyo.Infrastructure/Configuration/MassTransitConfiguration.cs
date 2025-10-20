using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oduyo.Infrastructure.Communication;

namespace Oduyo.Infrastructure.Configuration
{
    /// <summary>
    /// MassTransit yapılandırma extension methods
    /// </summary>
    public static class MassTransitConfiguration
    {
        /// <summary>
        /// MassTransit servislerini DI container'a ekler
        /// </summary>
        public static IServiceCollection AddMassTransitServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                // Consumer'ları ekle
                x.AddConsumer<SendEmailConsumer>();
                x.AddConsumer<SendSmsConsumer>();

                // RabbitMQ yapılandırması
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqConfig = configuration.GetSection("RabbitMQ");
                    var host = rabbitMqConfig["Host"] ?? "localhost";
                    var virtualHost = rabbitMqConfig["VirtualHost"] ?? "/";
                    var username = rabbitMqConfig["Username"] ?? "guest";
                    var password = rabbitMqConfig["Password"] ?? "guest";

                    cfg.Host(host, virtualHost, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    // Queue yapılandırmaları
                    cfg.ReceiveEndpoint("email-queue", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.ConfigureConsumer<SendEmailConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("sms-queue", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.ConfigureConsumer<SendSmsConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        /// <summary>
        /// MassTransit servislerini test/development ortamı için InMemory ile yapılandırır
        /// </summary>
        public static IServiceCollection AddMassTransitInMemory(
            this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                // Consumer'ları ekle
                x.AddConsumer<SendEmailConsumer>();
                x.AddConsumer<SendSmsConsumer>();

                // InMemory transport (test/development için)
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
