namespace DiscordSecretSanta.Core;

public class User
{
    public User(string name, string discordId, string avatarId, UserId userId)
    {
        Name = name;
        DiscordId = discordId;
        AvatarId = avatarId;
        UserId = userId;
    }

    public string Name { get; }
    public string DiscordId { get; }
    public string AvatarId { get; }
    public Uri WishlistUrl { get; set; }
    public UserId UserId { get; }
    public bool IsAdmin { get; set; }
}

public record UserId(string Value);