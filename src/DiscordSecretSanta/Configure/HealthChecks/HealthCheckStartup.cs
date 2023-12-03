using DiscordSecretSanta.Controllers;
using DiscordSecretSanta.Domain;

namespace DiscordSecretSanta.Configure.HealthChecks;

public static class HealthCheckStartup
{
    public static IServiceCollection HealthCheck(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .DomainHealthChecks();

        return services;
    }

    public static WebApplication HealthCheck(this WebApplication app)
    {
        app.MapHealthChecks(RootController.HealthRoute);
        return app;
    }
}