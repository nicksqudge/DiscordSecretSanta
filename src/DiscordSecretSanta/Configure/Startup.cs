using DiscordSecretSanta.Configure.HealthChecks;

namespace DiscordSecretSanta.Configure;

public static class Startup
{
    public static void AddDiscordSecretSantaServices(this IServiceCollection services)
    {
        services.AddHealthCheckServices();
    }

    public static void AddDiscordSecretSanta(this WebApplication app)
    {
        app.ConfigureHealthChecks();
    }
}