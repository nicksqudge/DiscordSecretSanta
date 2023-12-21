using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Controllers.Home;

public sealed record HomeUserResponse
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("isAdmin")]
    public bool IsAdmin { get; init; }
}