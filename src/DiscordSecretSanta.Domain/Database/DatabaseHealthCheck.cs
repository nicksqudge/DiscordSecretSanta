using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DiscordSecretSanta.Domain.Database;

internal class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDatabaseHealthChecks _dbHealthCheck;

    public DatabaseHealthCheck(IDatabaseHealthChecks dbHealthCheck)
    {
        _dbHealthCheck = dbHealthCheck;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var isHealthy = await _dbHealthCheck.CanConnectToDatabase();

        if (isHealthy)
            return HealthCheckResult.Healthy();

        return HealthCheckResult.Unhealthy("NO_DATABASE");
    }
}