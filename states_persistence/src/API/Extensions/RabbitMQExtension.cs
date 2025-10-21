using Application.DTOs.SettingsModels;
using Application.Messaging;
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
                busConfigurator.AddConsumer<StatesConsumer>();

                busConfigurator.UsingRabbitMq((context, configuration) =>
                {
                    RabbitMQSettings rabbitSettings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;

                    string host = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
                    ? "state.search.rabbitmq" // dentro do Docker
                    : "localhost";

                    configuration.Host(host, ushort.Parse(rabbitSettings.Port), "/", host =>
                    {
                        host.Username(rabbitSettings.Username);
                        host.Password(rabbitSettings.Password);
                    });


                    configuration.ReceiveEndpoint(rabbitSettings.Queue, endpointConfig =>
                    {
                        endpointConfig.ConfigureConsumeTopology = false;

                        endpointConfig.Bind("Application.DTOs.States.Messages:StatesMessage", bind =>
                        {
                            bind.ExchangeType = "fanout";
                        });

                        endpointConfig.ConfigureConsumer<StatesConsumer>(context);
                    });
                });
            });

            return service;
        }
    }
}
