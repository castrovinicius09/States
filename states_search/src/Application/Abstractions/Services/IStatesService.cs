using Application.Abstractions.Results;

namespace Application.Abstractions.Services
{
    public interface IStatesService
    {
        Task<Result> GetStatesAsync(CancellationToken cancellationToken = default);
    }
}
