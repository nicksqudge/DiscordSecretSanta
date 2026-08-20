namespace DiscordSecretSanta;

public class DefaultCampaignStatusService : ICampaignStatusService
{
    public bool CanDoArrived(CampaignStatusId id) => id == CampaignStatusId.Drawn;
}