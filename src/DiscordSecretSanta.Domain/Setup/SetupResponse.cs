using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Domain.Setup;

public sealed record SetupResponse
{
    [JsonPropertyName("configOk")]
    public bool ConfigOkay { get; init; }

    [JsonPropertyName("configDetail")]
    public ICollection<SetupConfigResponse>? ConfigDetails { get; init; } = null;

    [JsonPropertyName("user")]
    public SetupUserResponse? User { get; init; } = null;

    [JsonPropertyName("activeCampaign")]
    public SetupCampaignResponse? ActiveCampaign { get; init; } = null;
}