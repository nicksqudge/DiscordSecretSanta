namespace DiscordSecretSanta;

public sealed record SecretSantaMember(DiscordUserId UserId, Uri WishlistUrl)
{
    public DiscordUserId? SecretSantaId { get; set; }
}