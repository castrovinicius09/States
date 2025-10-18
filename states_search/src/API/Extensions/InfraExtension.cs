using API.SettingsModels;
using Infrastructure;
using Microsoft.Extensions.Options;
using Scrutor;

namespace API.Extensions
{
    public static class InfraExtension
    {
        public static void AddInfraDependencies(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.DI();
        }

        private static IServiceCollection AddHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("states", (serviceProvider, httpClient) =>
            {
                StateHttpSettings settings = serviceProvider.GetRequiredService<IOptions<StateHttpSettings>>().Value;

                httpClient.BaseAddress = new Uri(settings.BaseUrl);
            });

            return services;
        }

        private static IServiceCollection DI(this IServiceCollection services)
        {
            services.Scan(selector => selector
                .FromAssemblies(typeof(InfraAssembly).Assembly)
                .AddClasses(publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime());

            return services;
        }
    }
}
