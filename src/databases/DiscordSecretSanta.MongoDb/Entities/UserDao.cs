using System.Linq.Expressions;
using DiscordSecretSanta.Core;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DiscordSecretSanta.MongoDb.Entities;

public class UserDao
{
    public ObjectId Id { get; set; }
    
    public string UserId { get; set; }
    
    public string Name { get; set;  }
    
    public string DiscordId { get; set; }
    
    public string AvatarId { get; set;  }
    
    public string WishlistUrl { get; set; }
    
    public DateTimeOffset Created { get; set; }
    
    public bool IsAdmin { get; set; }
}