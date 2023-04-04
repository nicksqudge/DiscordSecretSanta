using CSharpFunctionalExtensions;

namespace DiscordSecretSanta.Core.Repositories;

public interface IUserRepository
{
    Task<Result> SaveUserWishlistUrl(UserId userId, Uri url, CancellationToken cancellationToken);
    Task<bool> DoesUserExist(UserId userId, CancellationToken cancellationToken);
    Task<Maybe<User>> GetUser(UserId userId, CancellationToken cancellationToken);
    Task<Result<User>> CreateUser(User user, CancellationToken cancellationToken);
    Task<Result> MakeUserAdmin(UserId userId, CancellationToken cancellationToken);
    Task<int> CountUsers(CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> ListUsers(CancellationToken cancellationToken);
}