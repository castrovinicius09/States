namespace Application.Abstractions
{
    public interface IMinIORepository
    {
        Task SaveJsonAsync(string statesList);
    }
}
