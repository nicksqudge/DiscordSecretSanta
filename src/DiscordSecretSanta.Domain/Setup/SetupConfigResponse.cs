using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Domain.Setup;

public sealed record SetupConfigResponse
{
    [JsonPropertyName("key")]
    public string Key { get; init; } = string.Empty;

    [JsonPropertyName("healthy")]
    public bool IsHealthy { get; init; }

    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;
}