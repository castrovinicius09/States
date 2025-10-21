using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerDependencies(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "State Persistence",
                    Version = "v1",
                    Description = "API para persistir lista de Estados brasileiros"
                });
            });

            return services;
        }
    }
}
