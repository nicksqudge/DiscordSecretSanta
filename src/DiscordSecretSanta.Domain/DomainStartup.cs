using DiscordSecretSanta.Domain.HealthCheck;
using DotnetCQRS.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Domain;

public static class DomainStartup
{
    public static IServiceCollection Domain(this IServiceCollection services)
    {
        return services
            .AddDotnetCqrs()
            .AddHandlersFromAssembly<Config>();
    }

    public static IHealthChecksBuilder DomainHealthChecks(this IHealthChecksBuilder builder)
    {
        builder.AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.HealthCheckKey);
        builder.AddCheck<DiscordStatusHealthCheck>(DiscordStatusHealthCheck.HealthCheckKey);

        return builder;
    }
}