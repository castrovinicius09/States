namespace Application.DTOs.States.Messages
{
    public class StatesMessage
    {
        public List<State> StatesList { get; set; } = new();
    }

    public class State
    {
        public int Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public Regiao Regiao { get; set; } = new Regiao();
    }

    public class Regiao
    {
        public int Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }
}
