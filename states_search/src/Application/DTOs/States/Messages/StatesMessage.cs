namespace Application.DTOs.States.Messages
{
    public class StatesMessage
    {
        public int Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public RegiaoMessage Regiao { get; set; } = new RegiaoMessage();
    }

    public class RegiaoMessage
    {
        public int Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }
}
