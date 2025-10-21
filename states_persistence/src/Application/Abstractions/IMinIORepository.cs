
namespace Application.Abstractions
{
    public interface IMinIORepository
    {
        Task<MemoryStream> GetFileByNameAsync(string nomeArquivo);
        Task<List<string>> GetFilesNamesListAsync(CancellationToken cancellationToken);
        Task SaveJsonAsync(string statesList);
    }
}
