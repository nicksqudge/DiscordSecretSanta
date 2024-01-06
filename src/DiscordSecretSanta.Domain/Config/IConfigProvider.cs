namespace DiscordSecretSanta.Domain.Config;

public interface IConfigProvider
{
    Task<AppConfig?> TryGetConfig(CancellationToken cancellationToken);

    Task<AppConfig> GetConfig(CancellationToken cancellationToken);
}