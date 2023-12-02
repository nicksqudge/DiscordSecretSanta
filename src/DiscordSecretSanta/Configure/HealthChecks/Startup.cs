using DiscordSecretSanta.Controllers;

namespace DiscordSecretSanta.Configure.HealthChecks;

public static class Startup
{
    public static void AddHealthCheckServices(this IServiceCollection services)
    {
        services.AddHealthChecks();
    }

    public static void ConfigureHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks(RootController.HealthRoute);
    }
}