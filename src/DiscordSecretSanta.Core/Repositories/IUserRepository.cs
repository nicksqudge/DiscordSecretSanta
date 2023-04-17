using CSharpFunctionalExtensions;

namespace DiscordSecretSanta.Core.Repositories;

public interface IUserRepository
{
    Task<Result> SaveUserWishlistUrl(UserId userId, Uri url, CancellationToken cancellationToken);
    Task<bool> DoesUserExist(UserId userId, CancellationToken cancellationToken);
    Task<Maybe<User>> GetUser(UserId userId, CancellationToken cancellationToken);
    Task<Result<User>> CreateUser(User user, CancellationToken cancellationToken);
    Task<Result> MakeUserAdmin(UserId userId, CancellationToken cancellationToken);
    Task<Result> RemoveUserAdmin(UserId userId, CancellationToken cancellationToken);
    Task<int> CountUsers(CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> ListUsers(CancellationToken cancellationToken);
    Task<Result> UpdateSecretSanta(UserId targetUserId, UserId secretSantaId, SecretSantaStatus status, CancellationToken cancellationToken);
    Task<Maybe<SecretSantaStatus>> GetStatusOfThisUsersGift(UserId targetUserId, CancellationToken cancellationToken);
    Task<Result> SetGifterOfUserStatus(UserId targetUserId, SecretSantaStatus status, CancellationToken cancellationToken);
}