using Application.DTOs.SettingsModels;

namespace API.Extensions
{
    public static class SettingExtension
    {
        public static IServiceCollection AddSettings(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
            service.Configure<MinioSettings>(configuration.GetSection("Minio"));

            return service;
        }
    }
}
