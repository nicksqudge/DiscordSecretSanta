namespace DiscordSecretSanta;

public interface DataStore
{
    Task<Status> GetStatus(CancellationToken cancellationToken);

    Task<int> GetNumberOfMembers(CancellationToken cancellationToken);
}