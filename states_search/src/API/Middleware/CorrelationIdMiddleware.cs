using Serilog.Context;

namespace API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string StatesCorrelationId = "CorrelationId";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId = Guid.NewGuid().ToString();

            context.Items[StatesCorrelationId] = correlationId;

            LogContext.PushProperty(StatesCorrelationId, correlationId);

            await _next(context);
        }
    }
}
