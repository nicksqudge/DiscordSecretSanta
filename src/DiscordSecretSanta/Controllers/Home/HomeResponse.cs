using System.Text.Json.Serialization;

namespace DiscordSecretSanta.Controllers.Home;

public sealed record HomeResponse
{
    [JsonPropertyName("configOk")]
    public bool ConfigOkay { get; init; }

    [JsonPropertyName("configDetail")]
    public ICollection<HomeConfigResponse>? ConfigDetails { get; init; } = null;

    [JsonPropertyName("user")]
    public HomeUserResponse? User { get; init; } = null;

    [JsonPropertyName("activeCampaign")]
    public HomeCampaignResponse? ActiveCampaign { get; init; } = null;
}