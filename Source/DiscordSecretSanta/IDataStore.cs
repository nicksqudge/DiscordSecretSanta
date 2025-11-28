namespace DiscordSecretSanta;

public interface IDataStore
{
    Task<Status> GetStatus(CancellationToken cancellationToken);

    Task<SecretSantaConfig> GetConfig(CancellationToken cancellationToken);
    
    Task SetStatus(Status status, CancellationToken cancellationToken);

    Task<int> GetNumberOfMembers(CancellationToken cancellationToken);
}