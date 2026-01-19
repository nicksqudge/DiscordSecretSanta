namespace DiscordSecretSanta;

public interface IWishlistUrlValidator
{
    Task<Uri?> IsValid(string url, CancellationToken cancellationToken);
}