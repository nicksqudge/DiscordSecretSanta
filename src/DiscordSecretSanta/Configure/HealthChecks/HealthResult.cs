using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Configure.HealthChecks;

public sealed record HealthResult
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("totalDuration")]
    public TimeSpan Duration { get; set; }

    [JsonPropertyName("entries")]
    public Dictionary<string, HealthInfo> Entries { get; set; } = new();

    public sealed record HealthInfo
    {
        [JsonPropertyName("data")]
        public object? Data { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("duration")]
        public TimeSpan Duration { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();
    }
}