using Application.Abstractions.Messaging;
using MassTransit;
using Serilog;

namespace Infrastructure.Messaging
{
    public sealed class StatesPublisher(
        IPublishEndpoint publishEndpoint,
        ILogger logger) : IMessageBus
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger _logger = logger;

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            _logger.Information("Enviando lista de estados...");

            await _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
