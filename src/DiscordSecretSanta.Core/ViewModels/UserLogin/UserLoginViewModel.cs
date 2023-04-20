namespace DiscordSecretSanta.Core.ViewModels.UserLogin;

public class UserLoginViewModel
{
    public string Title { get; set; } = string.Empty;
    public UserAndSecretSantaViewModel? User { get; set; } = null;
    public UserViewModel? SecretSanta { get; set; } = null;
    public SecretSantaStatus? PersonBuyingForThem { get; set; } = SecretSantaStatus.Unassigned;
    public string ErrorMessage { get; set; } = string.Empty;
    public bool HasUser => User is not null;
    public bool HasWishlist => !string.IsNullOrWhiteSpace(User?.WishlistUrl ?? string.Empty);
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
}