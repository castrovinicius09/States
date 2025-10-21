using Infrastructure;
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
