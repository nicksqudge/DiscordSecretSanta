using DiscordSecretSanta.Core;
using DiscordSecretSanta.Core.Repositories;
using DiscordSecretSanta.MongoDb.Entities;
using FluentResults;
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

    public async Task<Result<Uri>> GetUserWishlistUrl(UserId userId, CancellationToken cancellationToken)
    {
        var byUser = ByUserId(userId);

        var query = await _collection
            .Find(byUser)
            .Project(x => new
            {
                Id = x.UserId,
                WishlistUrl = x.WishlistUrl
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (query is null)
            return Result.Fail("User not found by id");

        return Result.Ok(new Uri(query.WishlistUrl));
    }

    public async Task<Result> SaveUserWishlistUrl(UserId userId, Uri url, CancellationToken cancellationToken)
    {
        var byUser = ByUserId(userId);

        var update = Builders<UserDao>.Update
            .Set(x => x.WishlistUrl, url.ToString());

        var result = await _collection.UpdateOneAsync(byUser, update, new UpdateOptions(), cancellationToken);
        if (result.ModifiedCount == 1)
            return Result.Ok();

        return Result.Fail("Could not update user");
    }

    private FilterDefinition<UserDao> ByUserId(UserId id)
    {
        return Builders<UserDao>.Filter
            .Eq(r => r.UserId, id.Value);
    }
}