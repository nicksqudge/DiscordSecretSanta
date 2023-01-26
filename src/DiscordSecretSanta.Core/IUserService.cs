namespace DiscordSecretSanta.Core;

public interface IUserService
{
    Task<User?> GetCurrentUser();
    Task<Result> UpdateWishlistUrl(User user, string url);
}