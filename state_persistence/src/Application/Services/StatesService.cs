using Application.Abstractions.Messaging;
using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Serilog;

namespace Application.Services
{
    internal sealed class StatesService(
        IMessageBus bus,
        ILogger logger) : IStatesService
    {
        private readonly IMessageBus _bus = bus;
        private readonly ILogger _logger = logger;

        public async Task<Result> GetStatesAsync(CancellationToken cancellationToken = default)
        {
            try
            {


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
