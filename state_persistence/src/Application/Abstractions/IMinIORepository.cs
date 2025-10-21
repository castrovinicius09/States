
namespace Application.Abstractions
{
    public interface IMinIORepository
    {
        Task<List<string>> GetFilesNamesListAsync(CancellationToken cancellationToken);
        Task SaveJsonAsync(string statesList);
    }
}
