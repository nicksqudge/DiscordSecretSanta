namespace DiscordSecretSanta.Domain.Status;

public sealed record StatusResponse
{
    public bool Installed { get; set; } = false;

    public bool CampaignSetup { get; set; } = false;
}