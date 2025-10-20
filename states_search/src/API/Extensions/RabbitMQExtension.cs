using Application.Abstractions.Messaging;
using Application.DTOs.SettingsModels;
using Infrastructure.Messaging;
using MassTransit;
using Microsoft.Extensions.Options;

namespace API.Extensions
{
    public static class RabbitMQExtension
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection service)
        {
            service.AddMassTransit(busConfigurator =>
            {
                busConfigurator.UsingRabbitMq((context, configuration) =>
                {
                    RabbitMQSettings rabbitSettings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;

                    configuration.Host(rabbitSettings.Host, host =>
                    {
                        host.Username(rabbitSettings.Username);
                        host.Password(rabbitSettings.Password);
                    });

                    configuration.ConfigureEndpoints(context);
                });
            });

            service.AddTransient<IMessageBus, StatesPublisher>();

            return service;
        }
    }
}
