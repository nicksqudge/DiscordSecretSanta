using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Domain.Database;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabaseHealthCheck<TInterface>(this IServiceCollection services)
        where TInterface : class, IDatabaseHealthChecks
    {
        services.AddTransient<IDatabaseHealthChecks, TInterface>();
        return services;
    }
}