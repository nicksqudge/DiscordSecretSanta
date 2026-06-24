namespace DiscordSecretSanta;

public sealed record SecretSantaMember(DiscordUserId UserId, Uri WishlistUrl)
{
    public DiscordUserId? SecretSantaId { get; set; }
    public SecretSantaStatus? SecretSantaStatus { get; set; }
}

public enum SecretSantaStatus
{
    Pending,
    Sent,
    Received
}