namespace DiscordSecretSanta.Core.ViewModels;

public class UserViewModel
{
    public string Name { get; set; } = string.Empty;
    public string DiscordTagId { get; set; } = string.Empty;
    public string AvatarId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;
    public string WishlistUrl { get; set; } = string.Empty;

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