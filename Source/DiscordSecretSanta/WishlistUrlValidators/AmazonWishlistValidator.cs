namespace DiscordSecretSanta.WishlistUrlValidators;

public class AmazonWishlistValidator : IWishlistUrlValidator
{
    public Task<Uri?> IsValid(string url, CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            Logger.Debug($"Invalid url {url}");
            return Task.FromResult<Uri?>(null);
        }

        return Task.FromResult(ValidateUrlText(uri));
    }

    private Uri? ValidateUrlText(Uri uri)
    {
        if (!uri.Authority.Contains(".amazon."))
        {
            Logger.Debug($"{uri}: Did not contain .amazon.");
            return null;
        }

        var wishlistPath = "/hz/wishlist/ls/";
        if (!uri.AbsolutePath.StartsWith(wishlistPath))
        {
            Logger.Debug($"{uri}: Did not contain start with hz/wishlist/ls");
            return null;
        }

        var wishlistId = uri.AbsolutePath.Replace(wishlistPath, "");
        if (wishlistId.Length < 3)
        {
            Logger.Debug($"{uri}: Contained invalid id length");
            return null;
        }

        return uri;
    }
}