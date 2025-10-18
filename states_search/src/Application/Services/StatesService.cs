using Application.Abstractions.HttpClients;
using Application.Abstractions.Messaging;
using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Application.DTOs.States.Messages;
using Application.DTOs.States.Responses;
using Application.Mapping;

namespace Application.Services
{
    internal sealed class StatesService(
        IStatesHttpClient statesHttpClient,
        IMessageBus bus) : IStatesService
    {
        private readonly IStatesHttpClient _statesHttpClient = statesHttpClient;
        private readonly IMessageBus _bus = bus;

        public async Task<Result> GetStatesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                List<StatesResponse> states = await _statesHttpClient.GetStatesAsync(cancellationToken);

                StatesMessage message = states.MapResponseToMessage();

                await _bus.PublishAsync(message);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
