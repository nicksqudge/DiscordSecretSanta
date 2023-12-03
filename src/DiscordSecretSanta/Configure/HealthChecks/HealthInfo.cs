namespace DiscordSecretSanta.Configure.HealthChecks;

public sealed record HealthInfo
{
    public string Key { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}