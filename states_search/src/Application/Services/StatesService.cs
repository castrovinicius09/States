using Application.Abstractions.HttpClients;
using Application.Abstractions.Messaging;
using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Application.DTOs.States.Messages;
using Application.DTOs.States.Responses;
using Application.Mapping;
using Serilog;

namespace Application.Services
{
    internal sealed class StatesService(
        IStatesHttpClient statesHttpClient,
        IMessageBus bus,
        ILogger logger) : IStatesService
    {
        private readonly IStatesHttpClient _statesHttpClient = statesHttpClient;
        private readonly IMessageBus _bus = bus;
        private readonly ILogger _logger = logger;

        public async Task<Result> GetStatesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                List<StatesResponse> states = await _statesHttpClient.GetStatesAsync(cancellationToken);

                _logger.Information("{0} estados localizados", states.Count);

                StatesMessage message = states.MapResponseToMessage();

                await _bus.PublishAsync(message);

                _logger.Information("Fim do processamento.");

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao buscar estados: {0}", ex.Message);

                return Result.Error(ex.Message);
            }
        }
    }
}
