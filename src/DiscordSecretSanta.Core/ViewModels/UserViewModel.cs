namespace DiscordSecretSanta.Core.ViewModels;

public class UserViewModel
{
    public string Name { get; set; } = string.Empty;
    public string DiscordTagId { get; set; } = string.Empty;
    public string AvatarId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;
    public string WishlistUrl { get; set; } = string.Empty;

    public bool HasWishlist => !string.IsNullOrWhiteSpace(WishlistUrl);
    

    public UserViewModel()
    {
        
    }

    public UserViewModel(User user)
    {
        Name = user.Name;
        WishlistUrl = user.WishlistUrl?.ToString() ?? string.Empty;
        AvatarId = user.AvatarId;
        DiscordTagId = user.DiscordId;
        UserId = user.UserId.Value;
        IsAdmin = user.IsAdmin;
    }
}

public class UserAndSecretSantaViewModel : UserViewModel
{
    public SecretSantaStatus SecretSantaStatus { get; set; } = SecretSantaStatus.Unassigned;
    public string SecretSantaUserId { get; set; } = string.Empty;
    
    public bool HasSecretSanta => SecretSantaStatus != SecretSantaStatus.Unassigned &&
                                  !string.IsNullOrWhiteSpace(SecretSantaUserId);
    
    public UserAndSecretSantaViewModel(User user) : base(user)
    {
        SecretSantaUserId = user.SecretSantaUserId?.Value ?? string.Empty;
        SecretSantaStatus = user.SecretSantaStatus;
    }
}