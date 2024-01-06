using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Domain.Setup;

public sealed record SetupUserResponse
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("isAdmin")]
    public bool IsAdmin { get; init; }
}