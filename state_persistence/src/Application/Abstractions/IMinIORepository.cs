namespace Application.Abstractions
{
    public interface IMinIORepository
    {
        Task SaveJsonAsync(MemoryStream statesList);
    }
}
