using Application.Abstractions;
using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Application.DTOs.States.Messages;
using Application.Mapping;
using Serilog;

namespace Application.Services
{
    internal sealed class StatesService(
        ILogger logger,
        IMinIORepository minioRepository) : IStatesService
    {
        private readonly ILogger _logger = logger;
        private readonly IMinIORepository _minioRepository = minioRepository;

        public async Task<Result> GetStatesFileNamesAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("Início da busca de arquivos no minIO");

                List<string> list = await _minioRepository.GetFilesNamesListAsync(cancellationToken);

                _logger.Information("Fim da busca");

                return Result.Success(list);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao fazer busca: {0}", ex.Message);

                return Result.Error(ex.Message);
            }
        }

        public async Task<Result> SaveStatesAsync(StatesMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Information("Início do processamento da mensagem");

                string jsonStatesList = message.MapMessageToJson();

                await _minioRepository.SaveJsonAsync(jsonStatesList);

                _logger.Information("Fim do processamento da mensagem");

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao processar mensagem: {0}", ex.Message);

                return Result.Error(ex.Message);
            }
        }
    }
}
