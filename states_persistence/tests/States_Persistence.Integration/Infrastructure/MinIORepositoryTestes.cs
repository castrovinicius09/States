using System.Text;
using System.Text.Json;
using Application.Abstractions;
using Application.DTOs.SettingsModels;
using Application.DTOs.States.Messages;
using Bogus;
using FluentAssertions;
using Infrastructure;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Serilog;
using Serilog.Core;

namespace States_Persistence.Integration.Infrastructure
{
    public class MinIORepositoryTestes
    {
        private readonly IMinioClient _client;
        private readonly IMinIORepository _repository;
        private readonly string _bucketName = "test-bucket";

        public MinIORepositoryTestes()
        {
            bool isCi = Environment.GetEnvironmentVariable("CI") == "true";

            IOptions<MinioSettings> settings = Options.Create(new MinioSettings
            {
                Endpoint = isCi ? "minio:9000" : "localhost:9000",
                AccessKey = "admin",
                SecretKey = "teste1234",
                BucketName = "test-bucket",
                ObjectName = "estados.json"
            });

            _client = new MinioClient()
                .WithEndpoint(settings.Value.Endpoint)
                .WithCredentials(settings.Value.AccessKey, settings.Value.SecretKey)
                .WithSSL(false)
                .Build();

            Logger logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            _repository = new MinIORepository(logger, _client, settings);
        }

        public async Task InitializeAsync()
        {
            // Garante que o bucket existe antes dos testes
            bool exists = await _client.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_bucketName));

            if (!exists)
            {
                await _client.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(_bucketName));
            }
        }

        public async Task DisposeAsync()
        {
            // Remove os objetos criados após os testes
            IObservable<Item> objects = _client.ListObjectsAsync(
                new ListObjectsArgs().WithBucket(_bucketName).WithRecursive(true));

            var completion = new TaskCompletionSource();

            var deleteTasks = new List<Task>();
            objects.Subscribe(item =>
            {
                deleteTasks.Add(_client.RemoveObjectAsync(
                    new RemoveObjectArgs().WithBucket(_bucketName).WithObject(item.Key)));
            },
            ex => completion.SetException(ex),
            () => completion.SetResult());

            await completion.Task;
            await Task.WhenAll(deleteTasks);
        }

        [Fact]
        public async Task SaveJsonAsync_ShouldUploadFileSuccessfully()
        {
            // Arrange
            string jsonStatesList = GenerateMockStatesMessageJson();

            // Act
            await _repository.SaveJsonAsync(jsonStatesList);

            // Assert
            List<string> files = await _repository.GetFilesNamesListAsync(CancellationToken.None);
            files.Should().NotBeEmpty();
            files.Should().Contain(f => f.Contains("estados.json"));
        }

        [Fact]
        public async Task GetFilesNamesListAsync_ShouldReturnFileNames()
        {
            // Arrange
            string jsonStatesList = GenerateMockStatesMessageJson();

            await _repository.SaveJsonAsync(jsonStatesList);

            // Act
            List<string> files = await _repository.GetFilesNamesListAsync(CancellationToken.None);

            // Assert
            files.Should().NotBeNullOrEmpty();
            files.Should().Contain(f => f.Contains("estados.json"));
        }

        [Fact]
        public async Task GetFileByNameAsync_ShouldReturnFileContent()
        {
            // Arrange
            string jsonStatesList = GenerateMockStatesMessageJson();

            await _repository.SaveJsonAsync(jsonStatesList);

            List<string> files = await _repository.GetFilesNamesListAsync(CancellationToken.None);
            string fileName = files.First();

            // Act
            MemoryStream fileStream = await _repository.GetFileByNameAsync(fileName);

            // Assert
            using var reader = new StreamReader(fileStream, Encoding.UTF8);
            string content = await reader.ReadToEndAsync();

            content.Should().NotBeEmpty();
        }

        public static string GenerateMockStatesMessageJson(int numberOfStates = 5)
        {
            var faker = new Faker();

            Faker<State> stateFaker = new Faker<State>()
                .RuleFor(s => s.Id, f => f.Random.Int(1, 1000))
                .RuleFor(s => s.Sigla, f => f.Address.StateAbbr())
                .RuleFor(s => s.Nome, f => f.Address.State())
                .RuleFor(s => s.Regiao, f => new Regiao
                {
                    Id = f.Random.Int(1, 100),
                    Sigla = f.Address.StateAbbr(),
                    Nome = f.Address.Country()
                });

            List<State> statesList = stateFaker.Generate(numberOfStates);

            var message = new StatesMessage
            {
                StatesList = statesList
            };

            return JsonSerializer.Serialize(message.StatesList, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}
