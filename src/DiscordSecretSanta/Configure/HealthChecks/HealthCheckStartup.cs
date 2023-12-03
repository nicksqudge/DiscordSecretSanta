using DiscordSecretSanta.Controllers;

namespace DiscordSecretSanta.Configure.HealthChecks;

public static class HealthCheckStartup
{
    public static IServiceCollection HealthCheck(this IServiceCollection services)
    {
        services.AddHealthChecks();
        return services;
    }

    public static WebApplication HealthCheck(this WebApplication app)
    {
        app.MapHealthChecks(RootController.HealthRoute);
        return app;
    }
}