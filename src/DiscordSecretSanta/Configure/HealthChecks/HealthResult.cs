using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Configure.HealthChecks;

public sealed record HealthResult
{
    [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;

    [JsonPropertyName("duration")] public TimeSpan Duration { get; set; }

    [JsonPropertyName("entries")] public ICollection<HealthInfo> Entries { get; set; } = new List<HealthInfo>();
}