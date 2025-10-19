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
                    Title = "State Search",
                    Version = "v1",
                    Description = "API ti get a Brazil states list"
                });
            });

            return services;
        }
    }
}
