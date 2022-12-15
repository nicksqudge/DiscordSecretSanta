namespace DiscordSecretSanta.Core;

public class User
{
    public User(string name, string discordId, string avatarId, string wishlistUrl, string userId)
    {
        Name = name;
        DiscordId = discordId;
        AvatarId = avatarId;
        WishlistUrl = wishlistUrl;
        UserId = userId;
    }

    public string Name { get; }
    public string DiscordId { get; }
    public string AvatarId { get; }
    public string WishlistUrl { get; }
    public string UserId { get; set; }
}