using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Controllers.Home;

public sealed record HomeCampaignResponse
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}