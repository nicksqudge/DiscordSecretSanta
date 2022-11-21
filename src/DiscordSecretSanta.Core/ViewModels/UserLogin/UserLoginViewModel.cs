namespace DiscordSecretSanta.Core.ViewModels.UserLogin;

public class UserLoginViewModel
{
    public string Title { get; set; } = string.Empty;
    public CurrentUser? User { get; set; } = null;
    public string WishlistUrl { get; set; } = string.Empty;

    public class CurrentUser
    {
        public string Name { get; set; } = string.Empty;
        public string DiscordTagId { get; set; } = string.Empty;
        public string AvatarId { get; set; } = string.Empty;
    }
}