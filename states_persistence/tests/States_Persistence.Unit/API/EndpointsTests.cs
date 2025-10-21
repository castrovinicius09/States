using Application.Abstractions.Results;
using Application.Abstractions.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NSubstitute;

namespace States_Persistence.Unit.API
{
    public class EndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly IStatesService _statesService;
        private readonly CancellationToken _cancellationToken;

        public EndpointsTests()
        {
            _statesService = Substitute.For<IStatesService>();
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task BuscarListaArquivos_WhenSuccess_ShouldReturnOkWithFileNames()
        {
            // Arrange
            string[] fileNames = new[] { "states1.json", "states2.json" };
            var expectedResult = Result.Success(fileNames);
            _statesService.GetStatesFileNamesAsync(_cancellationToken).Returns(expectedResult);

            // Act
            IResult response = await ExecuteBuscaListaEndpoint();

            // Assert
            response.Should().BeOfType<Ok<object>>();
            var okResult = response as Ok<object>;
            okResult?.Value.Should().BeEquivalentTo(fileNames);
        }

        [Fact]
        public async Task BuscarListaArquivos_WhenError_ShouldReturnProblemWithErrorMessage()
        {
            // Arrange
            string errorMessage = "Erro ao fazer busca";
            var expectedResult = Result.Error(errorMessage);
            _statesService.GetStatesFileNamesAsync(_cancellationToken).Returns(expectedResult);

            // Act
            IResult response = await ExecuteBuscaListaEndpoint();

            // Assert
            response.Should().BeOfType<ProblemHttpResult>();
            var problemResult = response as ProblemHttpResult;
            problemResult?.ProblemDetails.Detail.Should().Be(errorMessage);
        }

        [Fact]
        public async Task BaixarArquivos_WhenSuccess_ShouldReturnFileResult()
        {
            // Arrange
            string fileName = "state.json";
            var fileStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var expectedResult = Result.Success(fileStream);

            _statesService.GetFileByNameAsync(fileName, _cancellationToken).Returns(expectedResult);

            // Act
            IResult response = await ExecuteBaixarArquivoEndpoint(fileName);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeAssignableTo<IResult>();

            // Validate the File result properties
            var fileResult = response as FileStreamHttpResult;
            fileResult.Should().NotBeNull();
            fileResult?.FileStream.Should().BeSameAs(fileStream);
            fileResult?.ContentType.Should().Be("application/octet-stream");
            fileResult?.FileDownloadName.Should().Be(fileName);
        }

        [Fact]
        public async Task BaixarArquivos_WhenError_ShouldReturnProblemWithErrorMessage()
        {
            // Arrange
            string fileName = "example.txt";
            string errorMessage = "File not found";
            var expectedResult = Result.Error(errorMessage);
            _statesService.GetFileByNameAsync(fileName, _cancellationToken).Returns(expectedResult);

            // Act
            IResult response = await ExecuteBaixarArquivoEndpoint(fileName);

            // Assert
            response.Should().BeOfType<ProblemHttpResult>();
            var problemResult = response as ProblemHttpResult;
            problemResult?.ProblemDetails.Detail.Should().Be(errorMessage);
        }

        private async Task<IResult> ExecuteBuscaListaEndpoint()
        {
            Func<IStatesService, CancellationToken, Task<IResult>> endpointDelegate = (IStatesService service, CancellationToken token) =>
                service.GetStatesFileNamesAsync(token).ContinueWith(task =>
                    task.Result.IsSuccess
                        ? Results.Ok(task.Result.Data)
                        : Results.Problem(task.Result.ErrorMessage));

            return await endpointDelegate(_statesService, _cancellationToken);
        }

        private async Task<IResult> ExecuteBaixarArquivoEndpoint(string fileName)
        {
            Func<IStatesService, string, CancellationToken, Task<IResult>> endpointDelegate = (IStatesService service, string nomeArquivo, CancellationToken token) =>
                service.GetFileByNameAsync(nomeArquivo, token).ContinueWith(task =>
                    task.Result.IsSuccess
                        ? Results.File(fileStream: (Stream)task.Result.Data!, "application/octet-stream", nomeArquivo)
                        : Results.Problem(task.Result.ErrorMessage));

            return await endpointDelegate(_statesService, fileName, _cancellationToken);
        }
    }
}
