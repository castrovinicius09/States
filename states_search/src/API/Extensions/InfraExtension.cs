using API.SettingsModels;
using Application.Abstractions.HttpClients;
using Infrastructure;
using Infrastructure.HttpClients;
using Microsoft.Extensions.Options;
using Scrutor;

namespace API.Extensions
{
    public static class InfraExtension
    {
        public static IServiceCollection AddInfraDependencies(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.DI();

            return services;
        }

        private static IServiceCollection AddHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IStatesHttpClient, StatesHttpClient>((serviceProvider, httpClient) =>
            {
                StateHttpSettings settings = serviceProvider.GetRequiredService<IOptions<StateHttpSettings>>().Value;

                httpClient.BaseAddress = new Uri(settings.BaseUrl);
            })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new SocketsHttpHandler
                    {
                        PooledConnectionLifetime = TimeSpan.FromMinutes(1)
                    };
                })
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
                .AddStandardHedgingHandler();

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
