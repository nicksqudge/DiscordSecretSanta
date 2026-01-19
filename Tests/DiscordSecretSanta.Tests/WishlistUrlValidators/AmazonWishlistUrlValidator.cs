using DiscordSecretSanta.WishlistUrlValidators;

namespace DiscordSecretSanta.Tests.WishlistUrlValidators;

public class AmazonWishlistUrlValidatorTests
{
    private readonly IWishlistUrlValidator _subject = new AmazonWishlistValidator();
    
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("  ")]
    [TestCase("https://www.amazon.co.uk/hz/wishlist/ls/")]
    [TestCase("https://www.amazon.co.uk/hz/wishlist/")]
    [TestCase("https://www.amazon.co.uk/hz/")]
    [TestCase("https://www.amazon.co.uk/")]
    public async Task InvalidUrls(string url)
    {
        var result = await _subject.IsValid(url, CancellationToken.None);

        result.ShouldBeNull();
    }

    [Theory]
    [TestCase("https://www.amazon.co.uk/hz/wishlist/ls/35GHJDXGIAUDIAK?ref_=wl_share")]
    [TestCase("https://www.amazon.co.uk/hz/wishlist/ls/35GHJDXGIAUDIAK")]
    [TestCase("https://www.amazon.de/hz/wishlist/ls/35GHJDXGIAUDIAK")]
    public async Task ValidWishlistUrl(string url)
    {
        var result = await _subject.IsValid(url, CancellationToken.None);
        result.ShouldNotBeNull();
    }
}