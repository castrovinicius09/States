using System.Net.Http.Json;
using Application.Abstractions.HttpClients;
using Application.DTOs.States.Responses;

namespace Infrastructure.HttpClients
{
    internal sealed class StatesHttpClient : IStatesHttpClient
    {
        private readonly HttpClient _client;
        public StatesHttpClient(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("states");
        }

        public async Task<List<StatesResponse>?> GetStatesAsync(CancellationToken cancellationToken)
        {
            List<StatesResponse>? response = await _client.GetFromJsonAsync<List<StatesResponse>>("/localidades/estados", cancellationToken);

            return response;
        }
    }
}
