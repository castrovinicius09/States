using Application.Abstractions;
using Minio;
using Minio.DataModel.Args;
using Serilog;

namespace Infrastructure
{
    internal sealed class MinIORepository(
        ILogger logger,
        IMinioClient minioClient) : IMinIORepository
    {
        private const string _bucketName = "states_bucket";
        private const string _objectName = "states";

        private readonly ILogger _logger = logger;
        private readonly IMinioClient _minioClient = minioClient;

        public async Task SaveJsonAsync(MemoryStream statesList)
        {
            _logger.Information("Salvando arquivo JSON no MinIO...");

            BucketExistsArgs existingArgs = new BucketExistsArgs().WithBucket(_bucketName);
            if (!await _minioClient.BucketExistsAsync(existingArgs))
            {
                MakeBucketArgs newArgs = new MakeBucketArgs().WithBucket(_bucketName);

                await _minioClient.MakeBucketAsync(newArgs);
            }

            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(_objectName)
                .WithStreamData(statesList)
                .WithObjectSize(statesList.Length)
                .WithContentType("application/json"));

            _logger.Information("Arquivo JSON salvo com sucesso no MinIO.");
        }
    }
}
