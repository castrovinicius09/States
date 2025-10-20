using Application.Abstractions;
using Application.DTOs.SettingsModels;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Serilog;

namespace Infrastructure
{
    internal sealed class MinIORepository(
        ILogger logger,
        IMinioClient minioClient,
        IOptions<MinioSettings> settings) : IMinIORepository
    {
        private readonly ILogger _logger = logger;
        private readonly IMinioClient _minioClient = minioClient;
        private readonly MinioSettings _settings = settings.Value;

        public async Task SaveJsonAsync(MemoryStream statesList)
        {
            _logger.Information("Salvando arquivo JSON no MinIO...");

            BucketExistsArgs existingArgs = new BucketExistsArgs().WithBucket(_settings.BucketName);
            if (!await _minioClient.BucketExistsAsync(existingArgs))
            {
                MakeBucketArgs newArgs = new MakeBucketArgs().WithBucket(_settings.BucketName);

                await _minioClient.MakeBucketAsync(newArgs);
            }

            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(_settings.ObjectName)
                .WithStreamData(statesList)
                .WithObjectSize(statesList.Length)
                .WithContentType("application/json"));

            _logger.Information("Arquivo JSON salvo com sucesso no MinIO.");
        }
    }
}
