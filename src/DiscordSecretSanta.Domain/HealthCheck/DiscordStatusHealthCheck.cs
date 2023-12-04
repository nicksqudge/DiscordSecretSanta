using DiscordSecretSanta.Domain.Integrations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace DiscordSecretSanta.Domain.HealthCheck;

public class DiscordStatusHealthCheck : IHealthCheck
{
    public const string HealthCheckKey = "discord";
    private readonly IDiscordStatusApi _discordStatusApi;
    private readonly ILogger<DiscordStatusHealthCheck> _logger;

    public DiscordStatusHealthCheck(IDiscordStatusApi discordStatusApi, ILogger<DiscordStatusHealthCheck> logger)
    {
        _discordStatusApi = discordStatusApi;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var status = await _discordStatusApi.GetStatus();

        if (status == "All Systems Operational")
        {
            _logger.LogDebug("Discord status: {Status}", status);
            return HealthCheckResult.Healthy();
        }

        _logger.LogWarning("Discord status: {Status}", status);
        return HealthCheckResult.Unhealthy(status);
    }
}