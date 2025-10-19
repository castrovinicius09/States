using Application.Abstractions.HttpClients;
using Application.Abstractions.Messaging;
using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Application.DTOs.States.Messages;
using Application.DTOs.States.Responses;
using Application.Services;
using Bogus;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog;

namespace States_Search.Unit.Application
{
    public class StatesServiceTests
    {
        private readonly IStatesHttpClient _statesHttpClient;
        private readonly IMessageBus _messageBus;
        private readonly ILogger _logger;
        private readonly IStatesService _statesService;

        public StatesServiceTests()
        {
            _statesHttpClient = Substitute.For<IStatesHttpClient>();
            _messageBus = Substitute.For<IMessageBus>();
            _logger = Substitute.For<ILogger>();
            _statesService = new StatesService(_statesHttpClient, _messageBus, _logger);
        }

        [Fact]
        public async Task GetStatesAsync_ShouldReturnSuccess_WhenStatesAreFetchedSuccessfully()
        {
            // Arrange
            var faker = new Faker();
            var fakeStates = new List<StatesResponse>
            {
                new StatesResponse
                {
                    Id = faker.Random.Int(),
                    Sigla = faker.Address.StateAbbr(),
                    Nome = faker.Address.State(),
                    Regiao = new RegiaoResponse { Nome = faker.Address.Country() }
                }
            };

            _statesHttpClient.GetStatesAsync(Arg.Any<CancellationToken>()).Returns(fakeStates);

            // Act
            Result result = await _statesService.GetStatesAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            _logger.Received(1).Information("{0} estados localizados", fakeStates.Count);
            await _messageBus.Received(1).PublishAsync(Arg.Any<StatesMessage>());
            _logger.Received(1).Information("Fim do processamento.");
        }

        [Fact]
        public async Task GetStatesAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            // Arrange
            string exceptionMessage = "An error occurred";
            _statesHttpClient.GetStatesAsync(Arg.Any<CancellationToken>()).Throws(new Exception(exceptionMessage));

            // Act
            Result result = await _statesService.GetStatesAsync();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be(exceptionMessage);
            _logger.Received(1).Error(Arg.Any<Exception>(), "Erro ao buscar estados: {0}", exceptionMessage);
        }
    }
}
