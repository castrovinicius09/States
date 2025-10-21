using Application;
using Scrutor;

namespace API.Extensions
{
    public static class ApplicationExtesion
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.Scan(selector => selector
                .FromAssemblies(typeof(ApplicationAssembly).Assembly)
                .AddClasses(publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime());

            return services;
        }
    }
}
