namespace Domain.Entities.States
{
    public class State
    {
        public int ExternalId { get; }
        public string Code { get; }
        public string Name { get; }
        public Region Region { get; }
    }
}
