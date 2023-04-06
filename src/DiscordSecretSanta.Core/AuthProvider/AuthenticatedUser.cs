namespace DiscordSecretSanta.Core.AuthProvider;

public record AuthenticatedUser(string Name, string DiscordId, string AvatarId, UserId UserId)
{
    public User ToUser()
        => new User(Name, DiscordId, AvatarId, UserId);
}