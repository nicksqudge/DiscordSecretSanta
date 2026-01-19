namespace DiscordSecretSanta;

public interface IDataStore
{
    Task<bool> IsAdminInConfig(DiscordUserId userId, CancellationToken cancellationToken);
    
    Task<Status> GetStatus(CancellationToken cancellationToken);

    Task<SecretSantaConfig> GetConfig(CancellationToken cancellationToken);
    
    Task SetStatus(Status status, CancellationToken cancellationToken);

    Task<int> GetNumberOfMembers(CancellationToken cancellationToken);
    
    Task ToggleAdmin(DiscordUserId userId, bool isAdmin, CancellationToken cancellationToken);
    
    Task SetMaxPrice(string newMaxPrice, CancellationToken cancellationToken);
    Task AddMember(DiscordUserId discordUserId, Uri wishlistUrl, CancellationToken cancellationToken);
    Task<SecretSantaMember?> GetMember(DiscordUserId discordUserId, CancellationToken cancellationToken);
}