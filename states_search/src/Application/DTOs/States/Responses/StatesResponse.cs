namespace Application.DTOs.States.Responses
{
    public class StatesResponse
    {
        public int Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public RegiaoResponse Regiao { get; set; } = new RegiaoResponse();
    }

    public class RegiaoResponse
    {
        public int Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }
}
