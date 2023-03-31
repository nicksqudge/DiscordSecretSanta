using FluentResults;

namespace DiscordSecretSanta.Core;

public interface IUserService
{
    Task<User?> GetCurrentUser(CancellationToken cancellationToken);
    Task<Result> UpdateWishlistUrl(UserId user, Uri url, CancellationToken cancellationToken);
}