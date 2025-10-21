using Application.DTOs.Constants;
using Serilog.Context;

namespace API.Middleware
{
    public class CorrelationIdMiddleware(
        RequestDelegate next,
        Serilog.ILogger logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly Serilog.ILogger _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId = Guid.NewGuid().ToString();

            context.Items[ApplicationConstants.CorrelationId] = correlationId;

            LogContext.PushProperty(ApplicationConstants.CorrelationId, correlationId);

            _logger.Information("Início do busca de Estados do Brasil");

            await _next(context);
        }
    }
}
