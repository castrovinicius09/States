namespace API.SettingsModels
{
    public class MinioSettings
    {
        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
        public string BucketName { get; set; } = string.Empty;
    }
}
