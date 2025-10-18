using Application.Abstractions.HttpClients;
using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Application.DTOs.States.Responses;

namespace Application.Services
{
    internal sealed class StatesService : IStatesService
    {
        private readonly IStatesHttpClient _statesHttpClient;

        public StatesService(IStatesHttpClient statesHttpClient)
        {
            _statesHttpClient = statesHttpClient;
        }

        public async Task<Result> GetStatesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                List<StatesResponse> states = await _statesHttpClient.GetStatesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
