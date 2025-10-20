using Application.Abstractions.Services;
using Application.DTOs.Constants;
using MassTransit;
using Serilog;

namespace Application.Messaging
{
    public class StatesConsumer(
        ILogger logger,
        IStatesService statesService) : IConsumer<StatesMessage>
    {
        private readonly ILogger _logger = logger;
        private readonly IStatesService _statesService = statesService;

        public async Task Consume(ConsumeContext<StatesMessage> context)
        {
            string correlationId = context.Headers.Get<string>(ApplicationConstants.CorrelationId) ?? "no-correlation";

            StatesMessage message = context.Message;

            using (Serilog.Context.LogContext.PushProperty(ApplicationConstants.CorrelationId, correlationId))
            {
                _logger.Information("Mensagem recebida: {Count} estados", context.Message.StatesList.Count);

                await _statesService.SaveStatesAsync(message, context.CancellationToken);
            }
        }
    }
}
