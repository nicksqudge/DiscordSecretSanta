namespace DiscordSecretSanta.Domain.Integrations;

public interface IDiscordStatusApi
{
    Task<string> GetStatus();
}