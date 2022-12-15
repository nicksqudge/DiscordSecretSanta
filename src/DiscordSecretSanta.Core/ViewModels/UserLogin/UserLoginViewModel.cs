namespace DiscordSecretSanta.Core.ViewModels.UserLogin;

public class UserLoginViewModel
{
    public string Title { get; set; } = string.Empty;
    public CurrentUser? User { get; set; } = null;
    public string WishlistUrl { get; set; } = string.Empty;
    public bool HasUser => User is not null;
    public bool HasWishlist => !string.IsNullOrWhiteSpace(WishlistUrl);

    public class CurrentUser
    {
        public string Name { get; set; } = string.Empty;
        public string DiscordTagId { get; set; } = string.Empty;
        public string AvatarId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}