using CSharpFunctionalExtensions;
using DiscordSecretSanta.Core;
using DiscordSecretSanta.Core.Repositories;
using DiscordSecretSanta.MongoDb.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DiscordSecretSanta.MongoDb;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserDao> _collection;

    public UserRepository(IMongoDatabase database)
    {
        _collection = database
            .GetCollection<UserDao>("users");
    }

    public Task<Result> SaveUserWishlistUrl(UserId userId, Uri url, CancellationToken cancellationToken)
    {
        return UpdateUser(
            userId,
            Builders<UserDao>.Update.Set(x => x.WishlistUrl, url.ToString()),
            cancellationToken
        );
    }

    public Task<bool> DoesUserExist(UserId userId, CancellationToken cancellationToken)
    {
        return FindUser(userId).AnyAsync(cancellationToken);
    }

    public async Task<Maybe<User>> GetUser(UserId userId, CancellationToken cancellationToken)
    {
        var user = await FindUser(userId).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Maybe<User>.None;

        return Project(user);
    }

    public async Task<Result<User>> CreateUser(User user, CancellationToken cancellationToken)
    {
        var dao = new UserDao()
        {
            WishlistUrl = user.WishlistUrl?.ToString() ?? string.Empty,
            Name = user.Name,
            AvatarId = user.AvatarId,
            DiscordId = user.DiscordId,
            UserId = user.UserId.Value,
            Created = DateTimeOffset.UtcNow,
            IsAdmin = user.IsAdmin
        };

        await _collection.InsertOneAsync(dao, cancellationToken);
        return Result.Success(user);
    }

    public Task<Result> MakeUserAdmin(UserId userId, CancellationToken cancellationToken)
    {
        return UpdateUser(
            userId,
            Builders<UserDao>.Update.Set(x => x.IsAdmin, true),
            cancellationToken
        );
    }

    public async Task<int> CountUsers(CancellationToken cancellationToken)
    {
        var count = await _collection.CountDocumentsAsync(new BsonDocument(), null, cancellationToken);
        return (int)count;
    }

    public async Task<IReadOnlyList<User>> ListUsers(CancellationToken cancellationToken)
    {
        var users = await _collection
                .Find(_ => true)
                .ToListAsync(cancellationToken);

        if (users.Any())
            return users.Select(x => Project(x)).ToList();

        return new List<User>();
    }

    private FilterDefinition<UserDao> ByUserId(UserId id)
    {
        return Builders<UserDao>.Filter
            .Eq(r => r.UserId, id.Value);
    }

    private IFindFluent<UserDao, UserDao> FindUser(UserId userId)
    {
        return _collection.Find(ByUserId(userId));
    }

    private async Task<Result> UpdateUser(UserId userId, UpdateDefinition<UserDao> update,
        CancellationToken cancellationToken)
    {
        var result = await _collection.UpdateOneAsync(ByUserId(userId), update, new UpdateOptions(), cancellationToken);
        if (result.ModifiedCount == 1)
            return Result.Ok();

        return Result.Fail("Could not update user");
    }
    
    private User Project(UserDao dao)
     => new User(dao.Name, dao.DiscordId, dao.AvatarId, new UserId(dao.UserId))
     {
         IsAdmin = dao.IsAdmin,
         WishlistUrl = string.IsNullOrWhiteSpace(dao.WishlistUrl) ? null : new Uri(dao.WishlistUrl)
     };
}