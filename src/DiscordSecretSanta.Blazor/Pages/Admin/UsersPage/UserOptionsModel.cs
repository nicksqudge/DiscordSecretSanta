using DiscordSecretSanta.Core;

namespace DiscordSecretSanta.Blazor.Pages.Admin.UsersPage;

public class UserOptionsModel
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public bool IsAdmin { get; set; }
    public bool HasWishlist { get; set; }
    public string WishlistUrl { get; set; }
}