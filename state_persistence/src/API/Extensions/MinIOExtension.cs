using API.SettingsModels;
using Minio;

namespace API.Extensions
{
    public static class MinIOExtension
    {
        public static IServiceCollection AddMinIODependencies(this IServiceCollection service, IConfiguration configuration)
        {
            MinioSettings? minioSettings = configuration.GetSection("Minio").Get<MinioSettings>();

            if (minioSettings is null)
            {
                throw new KeyNotFoundException("Configs para minIO não localizadas");
            }

            service.AddMinio(configureClient => configureClient
                .WithEndpoint(minioSettings.Endpoint)
                .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
                .Build());

            return service;
        }
    }
}
