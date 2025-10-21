using Application.Abstractions.Messaging;
using Application.DTOs.Constants;
using Application.DTOs.SettingsModels;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.Messaging
{
    public sealed class StatesPublisher(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        ISendEndpointProvider sendEndpointProvider,
        IOptions<RabbitMQSettings> settings) : IMessageBus
    {
        private readonly ILogger _logger = logger;
        private readonly ISendEndpointProvider _sendEndpointProvider = sendEndpointProvider;
        private readonly RabbitMQSettings _settings = settings.Value;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            string? correlationId = _httpContextAccessor.HttpContext?.Items[ApplicationConstants.CorrelationId]?.ToString();

            _logger.Information("Enviando lista de estados para persistência");

            ISendEndpoint endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{_settings.Queue}"));

            await endpoint.Send(message, context =>
            {
                context.Headers.Set(ApplicationConstants.CorrelationId, correlationId);
            });
        }
    }
}
