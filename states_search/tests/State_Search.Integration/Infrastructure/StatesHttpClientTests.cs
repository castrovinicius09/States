using System.Net;
using System.Net.Http.Json;
using Application.Abstractions.HttpClients;
using Application.DTOs.States.Responses;
using Bogus;
using FluentAssertions;
using Infrastructure.HttpClients;
using Microsoft.Extensions.DependencyInjection;
using State_Search.Integration.TestData;

namespace State_Search.Integration.Infrastructure
{
    public class StatesHttpClientTests
    {
        private readonly IStatesHttpClient _statesHttpClient;

        private readonly List<StatesResponse> fakeStates = new Faker<StatesResponse>()
                .RuleFor(s => s.Id, f => f.Random.Int(1, 100))
                .RuleFor(s => s.Sigla, f => f.Address.StateAbbr())
                .RuleFor(s => s.Nome, f => f.Address.State())
                .RuleFor(s => s.Regiao, f => new RegiaoResponse
                {
                    Id = f.Random.Int(1, 10),
                    Nome = f.Address.City()
                })
                .Generate(5);

        public StatesHttpClientTests()
        {
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(fakeStates)
            };

            var handler = new MockHttpMessageHandler(_ => httpResponse);
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://fakeapi.com/")
            };

            var services = new ServiceCollection();
            services.AddSingleton(httpClient);
            services.AddSingleton<IStatesHttpClient, StatesHttpClient>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            _statesHttpClient = serviceProvider.GetRequiredService<IStatesHttpClient>();
        }

        [Fact]
        public async Task GetStatesAsync_ShouldReturnListOfStates_WhenApiReturnsValidResponse()
        {
            // Arrange
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            List<StatesResponse> result = await _statesHttpClient.GetStatesAsync(cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().BeEquivalentTo(fakeStates);
        }
    }
}
