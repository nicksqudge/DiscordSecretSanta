using DiscordSecretSanta.Configure.HealthChecks;
using DiscordSecretSanta.Domain;

namespace DiscordSecretSanta.Configure;

public static class Startup
{
    public static void ConfigureDiscordSecretSantaServices(this IServiceCollection services)
    {
        services
            .Domain()
            .HealthCheck();
    }

    public static void ConfigureDiscordSecretSanta(this WebApplication app)
    {
        app
            .HealthCheck();
    }
}