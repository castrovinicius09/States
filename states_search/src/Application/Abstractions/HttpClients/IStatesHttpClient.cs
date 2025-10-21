using Application.DTOs.States.Responses;

namespace Application.Abstractions.HttpClients
{
    public interface IStatesHttpClient
    {
        Task<List<StatesResponse>> GetStatesAsync(CancellationToken cancellationToken);
    }
}
