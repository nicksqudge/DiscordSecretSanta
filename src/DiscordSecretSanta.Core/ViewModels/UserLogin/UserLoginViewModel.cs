namespace DiscordSecretSanta.Core.ViewModels.UserLogin;

public class UserLoginViewModel
{
    public string Title { get; set; } = string.Empty;
    public UserViewModel? User { get; set; } = null;
    public string ErrorMessage { get; set; } = string.Empty;
    public bool HasUser => User is not null;
    public bool HasWishlist => !string.IsNullOrWhiteSpace(User?.WishlistUrl ?? string.Empty);
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
}