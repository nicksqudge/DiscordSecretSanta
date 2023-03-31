using FluentResults;

namespace DiscordSecretSanta.Core.Repositories;

public interface IUserRepository
{
    Task<Result<Uri>> GetUserWishlistUrl(UserId userId, CancellationToken cancellationToken);
    Task<Result> SaveUserWishlistUrl(UserId userId, Uri url, CancellationToken cancellationToken);
}