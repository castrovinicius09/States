using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Application.Mapping;
using Application.Messaging;
using Serilog;

namespace Application.Services
{
    internal sealed class StatesService(ILogger logger) : IStatesService
    {
        private readonly ILogger _logger = logger;

        public async Task<Result> SaveStatesAsync(StatesMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Information("Início do processamento da mensagem.");

                var jsonStatesList = message.MapMessageToJson();

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao processar mensagem: {0}", ex.Message);

                return Result.Error(ex.Message);
            }
        }
    }
}
