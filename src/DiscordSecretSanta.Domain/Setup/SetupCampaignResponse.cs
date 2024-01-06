using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Domain.Setup;

public sealed record SetupCampaignResponse
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}