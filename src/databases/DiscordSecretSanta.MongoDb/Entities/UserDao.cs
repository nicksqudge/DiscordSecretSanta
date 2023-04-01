using MongoDB.Bson;

namespace DiscordSecretSanta.MongoDb.Entities;

public class UserDao
{
    public ObjectId Id { get; set; }
    
    public string UserId { get; set; }
    
    public string Name { get; }
    
    public string DiscordId { get; }
    
    public string AvatarId { get; }
    
    public string WishlistUrl { get; set; }
    
    public DateTimeOffset Created { get; set; }
}