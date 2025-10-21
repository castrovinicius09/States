using Application.Abstractions.Results;
using Application.DTOs.States.Messages;

namespace Application.Abstractions.Services
{
    public interface IStatesService
    {
        Task<Result> GetStatesFileNamesAsync(CancellationToken cancellationToken);
        Task<Result> SaveStatesAsync(StatesMessage message, CancellationToken cancellationToken = default);
    }
}
