namespace DiscordSecretSanta;

public interface IDataStore
{
    Task<bool> IsAdminInConfig(DiscordUserId userId, CancellationToken cancellationToken);
    
    Task<CampaignStatusId> GetStatus(CancellationToken cancellationToken);

    Task<SecretSantaConfig> GetConfig(CancellationToken cancellationToken);
    
    Task SetStatus(CampaignStatusId status, CancellationToken cancellationToken);

    Task<SecretSantaMember?> GetMember(DiscordUserId discordUserId, CancellationToken cancellationToken);
    Task<SecretSantaMember?> GetMembersSecretSanta(DiscordUserId discordUserId, CancellationToken cancellationToken);
    Task<int> GetNumberOfMembers(CancellationToken cancellationToken);
    Task<SecretSantaMember[]> GetMembers(CancellationToken cancellationToken);
    
    Task ToggleAdmin(DiscordUserId userId, bool isAdmin, CancellationToken cancellationToken);
    
    Task SetMaxPrice(string newMaxPrice, CancellationToken cancellationToken);
    Task AddMember(DiscordUserId discordUserId, Uri wishlistUrl, CancellationToken cancellationToken);
    Task SetSecretSanta(DiscordUserId targetUser, DiscordUserId secretSanta, CancellationToken cancellationToken);
    Task SetSecretSantaStatus(DiscordUserId targetUser, SecretSantaStatus status, CancellationToken cancellationToken);
}