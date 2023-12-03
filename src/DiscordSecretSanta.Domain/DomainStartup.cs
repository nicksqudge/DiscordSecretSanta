using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Domain;

public static class DomainStartup
{
    public static IServiceCollection Domain(this IServiceCollection services)
    {
        return services;
    }
}