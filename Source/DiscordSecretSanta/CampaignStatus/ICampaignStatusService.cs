namespace DiscordSecretSanta;

public interface ICampaignStatusService
{
    bool CanDoArrived(CampaignStatusId id);
}