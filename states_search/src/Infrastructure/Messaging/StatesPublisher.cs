using Application.Abstractions.Messaging;
using Application.DTOs.Constants;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Infrastructure.Messaging
{
    public sealed class StatesPublisher(
        IPublishEndpoint publishEndpoint,
        ILogger logger,
        IHttpContextAccessor httpContextAccessor) : IMessageBus
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger _logger = logger;

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            string? correlationId = _httpContextAccessor.HttpContext?.Items[ApplicationConstants.CorrelationId]?.ToString();

            _logger.Information("Enviando lista de estados para persistência");

            await _publishEndpoint.Publish(message, context =>
            {
                context.Headers.Set(ApplicationConstants.CorrelationId, correlationId);
            });
        }
    }
}
