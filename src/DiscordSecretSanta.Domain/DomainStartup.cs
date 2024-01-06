using DiscordSecretSanta.Domain.Config;
using DiscordSecretSanta.Domain.HealthCheck;
using DotnetCQRS.Extensions.Microsoft.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Domain;

public static class DomainStartup
{
    public static IServiceCollection Domain(this IServiceCollection services)
    {
        var thisAssembly = typeof(AppConfig).Assembly;

        return services
            .AddDotnetCqrs()
            .AddHandlersFromAssembly(thisAssembly)
            .AddValidatorsFromAssembly(thisAssembly);
    }

    public static IHealthChecksBuilder DomainHealthChecks(this IHealthChecksBuilder builder)
    {
        builder.AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.HealthCheckKey);
        builder.AddCheck<DiscordStatusHealthCheck>(DiscordStatusHealthCheck.HealthCheckKey);

        return builder;
    }
}