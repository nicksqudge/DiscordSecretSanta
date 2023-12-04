using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace DiscordSecretSanta.Domain.HealthCheck;

internal class DatabaseHealthCheck : IHealthCheck
{
    public const string HealthCheckKey = "database";
    private readonly IDatabaseHealthChecks _dbHealthCheck;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(IDatabaseHealthChecks dbHealthCheck, ILogger<DatabaseHealthCheck> logger)
    {
        _dbHealthCheck = dbHealthCheck;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var isHealthy = await _dbHealthCheck.CanConnectToDatabase();

        if (isHealthy)
        {
            _logger.LogDebug("Database connection is healthy");
            return HealthCheckResult.Healthy();
        }

        _logger.LogError("No database connection");
        return HealthCheckResult.Unhealthy("NO_DATABASE");
    }
}