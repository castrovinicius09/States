using System.Text.Json;
using Application.Abstractions;
using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Application.DTOs.States.Messages;
using Application.Services;
using Bogus;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog;

namespace States_Persistence.Unit.Application
{
    public class StatesServiceTests
    {
        private readonly IMinIORepository _minioRepository;
        private readonly ILogger _logger;
        private readonly IStatesService _statesService;

        public StatesServiceTests()
        {
            _minioRepository = Substitute.For<IMinIORepository>();
            _logger = Substitute.For<ILogger>();
            _statesService = new StatesService(_logger, _minioRepository);
        }

        [Fact]
        public async Task GetFileByNameAsync_ShouldReturnSuccess_WhenFileIsFetchedSuccessfully()
        {
            // Arrange
            string fileName = "state.json";
            var fileStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            _minioRepository.GetFileByNameAsync(fileName).Returns(fileStream);

            // Act
            Result result = await _statesService.GetFileByNameAsync(fileName, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(fileStream);
            _logger.Received(1).Information("Início da busca de arquivo pelo nome {0}", fileName);
            _logger.Received(1).Information("Fim da busca");
        }

        [Fact]
        public async Task GetFileByNameAsync_ShouldReturnError_WhenFileIsNotFound()
        {
            // Arrange
            string fileName = "state.json";

            _minioRepository.GetFileByNameAsync(fileName).Returns((MemoryStream)null);

            // Act
            Result result = await _statesService.GetFileByNameAsync(fileName, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Erro ao buscar arquivo {0}", fileName);
            _logger.Received(1).Information("Início da busca de arquivo pelo nome {0}", fileName);
        }

        [Fact]
        public async Task GetStatesFileNamesAsync_ShouldReturnSuccess_WhenFileNamesAreFetchedSuccessfully()
        {
            // Arrange
            var fileNames = new List<string> { "state1.json", "state2.json" };
            _minioRepository.GetFilesNamesListAsync(Arg.Any<CancellationToken>()).Returns(fileNames);

            // Act
            Result result = await _statesService.GetStatesFileNamesAsync(CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(fileNames);
            _logger.Received(1).Information("Início da busca de arquivos no minIO");
            _logger.Received(1).Information("Fim da busca");
        }

        [Fact]
        public async Task GetStatesFileNamesAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            // Arrange
            string exceptionMessage = "An error occurred";
            _minioRepository.GetFilesNamesListAsync(Arg.Any<CancellationToken>()).Throws(new Exception(exceptionMessage));

            // Act
            Result result = await _statesService.GetStatesFileNamesAsync(CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be(exceptionMessage);
            _logger.Received(1).Error(Arg.Any<Exception>(), "Erro ao fazer busca: {0}", exceptionMessage);
        }

        [Fact]
        public async Task SaveStatesAsync_ShouldReturnSuccess_WhenStatesAreSavedSuccessfully()
        {
            // Arrange
            StatesMessage message = GenerateMockStatesMessage();
            string jsonStatesList = JsonSerializer.Serialize(message.StatesList, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            // Act
            Result result = await _statesService.SaveStatesAsync(message, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _logger.Received(1).Information("Início do processamento da mensagem");
            await _minioRepository.Received(1).SaveJsonAsync(jsonStatesList);
            _logger.Received(1).Information("Fim do processamento da mensagem");
        }

        [Fact]
        public async Task SaveStatesAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            // Arrange
            var message = new StatesMessage();
            string exceptionMessage = "An error occurred";
            _minioRepository.SaveJsonAsync(Arg.Any<string>()).Throws(new Exception(exceptionMessage));

            // Act
            Result result = await _statesService.SaveStatesAsync(message, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be(exceptionMessage);
            _logger.Received(1).Error(Arg.Any<Exception>(), "Erro ao processar mensagem: {0}", exceptionMessage);
        }

        public static StatesMessage GenerateMockStatesMessage(int numberOfStates = 5)
        {
            var faker = new Faker();

            Faker<State> stateFaker = new Faker<State>()
                .RuleFor(s => s.Id, f => f.Random.Int(1, 1000))
                .RuleFor(s => s.Sigla, f => f.Address.StateAbbr())
                .RuleFor(s => s.Nome, f => f.Address.State())
                .RuleFor(s => s.Regiao, f => new Regiao
                {
                    Id = f.Random.Int(1, 100),
                    Sigla = f.Address.StateAbbr(),
                    Nome = f.Address.Country()
                });

            List<State> statesList = stateFaker.Generate(numberOfStates);

            return new StatesMessage
            {
                StatesList = statesList
            };
        }
    }
}
