namespace DiscordSecretSanta.Core;

public interface IUserService
{
    Task<User?> GetCurrentUser();
}