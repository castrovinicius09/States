using Application.Abstractions.Results;
using Application.Abstractions.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NSubstitute;

namespace States_Search.Unit.API
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
        public async Task BuscarEstados_WhenSuccess_ShouldReturnOkWithSuccessMessage()
        {
            // Arrange
            var expectedResult = Result.Success();
            _statesService.GetStatesAsync(_cancellationToken).Returns(expectedResult);

            // Act
            IResult response = await ExecuteEndpoint();

            // Assert
            response.Should().BeOfType<Ok<string>>();
            var okResult = response as Ok<string>;
            okResult?.Value.Should().Be("Lista de Estados enviada");
        }

        [Fact]
        public async Task BuscarEstados_WhenError_ShouldReturnProblemWithErrorMessage()
        {
            // Arrange
            string errorMessage = "Error fetching states";
            var expectedResult = Result.Error(errorMessage);

            _statesService.GetStatesAsync(_cancellationToken).Returns(expectedResult);

            // Act
            IResult response = await ExecuteEndpoint();

            // Assert
            response.Should().BeOfType<ProblemHttpResult>();
            var problemResult = response as ProblemHttpResult;
            problemResult?.ProblemDetails.Detail.Should().Be(errorMessage);
        }

        private async Task<IResult> ExecuteEndpoint()
        {
            Func<IStatesService, CancellationToken, Task<IResult>> endpointDelegate = (IStatesService service, CancellationToken token) =>
                service.GetStatesAsync(token).ContinueWith(task =>
                    task.Result.IsSuccess
                        ? Results.Ok("Lista de Estados enviada")
                        : Results.Problem(task.Result.ErrorMessage));

            return await endpointDelegate(_statesService, _cancellationToken);
        }
    }
}
