using Application.Abstractions.Results;
using Application.Messaging;

namespace Application.Abstractions.Services
{
    public interface IStatesService
    {
        Task<Result> SaveStatesAsync(StatesMessage message, CancellationToken cancellationToken = default);
    }
}
