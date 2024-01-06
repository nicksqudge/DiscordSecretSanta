namespace DiscordSecretSanta.Domain;

public sealed record Config;

public interface IConfigProvider
{
    Task<Config?> TryGetConfig(CancellationToken cancellationToken);

    Task<Config> GetConfig(CancellationToken cancellationToken);
}