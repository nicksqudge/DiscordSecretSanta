using DiscordSecretSanta.Controllers;
using DiscordSecretSanta.Domain;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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
        app.MapHealthChecks(HealthCheckController.Route, new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }
}