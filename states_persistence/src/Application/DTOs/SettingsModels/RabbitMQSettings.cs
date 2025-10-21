namespace Application.DTOs.SettingsModels
{
    public class RabbitMQSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Queue { get; set; } = string.Empty;
    }
}
