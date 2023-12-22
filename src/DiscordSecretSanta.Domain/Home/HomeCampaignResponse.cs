using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Domain.Home;

public sealed record HomeCampaignResponse
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}