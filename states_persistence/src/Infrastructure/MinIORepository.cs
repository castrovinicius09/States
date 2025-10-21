using System.Text;
using Application.Abstractions;
using Application.DTOs.SettingsModels;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Serilog;

namespace Infrastructure
{
    internal sealed class MinIORepository : IMinIORepository
    {
        private readonly ILogger _logger;
        private readonly IMinioClient _minioClient;
        private readonly MinioSettings _settings;

        public MinIORepository(
            ILogger logger,
            IMinioClient minioClient,
            IOptions<MinioSettings> settings)
        {
            _logger = logger;
            _minioClient = minioClient;
            _settings = settings.Value;

            BucketExistsArgs existingArgs = new BucketExistsArgs().WithBucket(_settings.BucketName);
            if (!_minioClient.BucketExistsAsync(existingArgs).Result)
            {
                MakeBucketArgs newArgs = new MakeBucketArgs().WithBucket(_settings.BucketName);

                _minioClient.MakeBucketAsync(newArgs);
            }
        }

        public async Task<MemoryStream> GetFileByNameAsync(string nomeArquivo)
        {
            _logger.Information("Buscar arquivo {0} no bucket {1}", nomeArquivo, _settings.BucketName);

            var stream = new MemoryStream();

            await _minioClient.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(_settings.BucketName)
                    .WithObject(nomeArquivo)
                    .WithCallbackStream(s => s.CopyTo(stream)));

            stream.Position = 0;
            return stream;
        }

        public async Task<List<string>> GetFilesNamesListAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Buscar lista de arquivos no bucket {0}", _settings.BucketName);

            var files = new List<string>();

            ListObjectsArgs listArgs = new ListObjectsArgs()
                .WithBucket(_settings.BucketName)
                .WithRecursive(true);

            IObservable<Item> observable = _minioClient.ListObjectsAsync(listArgs);

            var completion = new TaskCompletionSource();

            IDisposable subscription = observable.Subscribe(
                item =>
                {
                    files.Add(item.Key);
                },
                ex =>
                {
                    _logger.Error(ex, "Erro ao listar arquivos do MinIO.");
                    completion.SetException(ex);
                },
                () =>
                {
                    completion.SetResult();
                });

            await completion.Task;

            _logger.Information("Fim da busca no bucket {0}", _settings.BucketName);

            return files;
        }

        public async Task SaveJsonAsync(string statesList)
        {
            _logger.Information("Salvando arquivo JSON no MinIO...");

            using var streamStates = new MemoryStream(Encoding.UTF8.GetBytes(statesList));

            _logger.Information("Enviando arquivo {ObjectName} com {Length} bytes para o bucket {BucketName}.",
                _settings.ObjectName, streamStates.Length, _settings.BucketName);

            //para evitar persistencia e incremento automatico apenas para testes
            var random = new Random();
            string randomNumber = random.Next(0, 1000).ToString();

            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject($"{_settings.ObjectName}{DateTime.UtcNow.Date.ToString("ddMMyyyy")}_{randomNumber}")
                .WithStreamData(streamStates)
                .WithObjectSize(streamStates.Length)
                .WithContentType("application/json"));

            _logger.Information("Arquivo {ObjectName} salvo com sucesso no MinIO.", _settings.ObjectName);
        }
    }
}
