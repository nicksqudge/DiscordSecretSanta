namespace DiscordSecretSanta.Core.ViewModels.UserLogin;

public interface IUserLoginViewHandler
{
    Task<UserLoginViewModel> OnInitAsync();
    Task<UserLoginViewModel> SetWishlistUrl(string url);
}

public class UserLoginViewHandler : IUserLoginViewHandler
{
    private readonly ISetupService _setupService;
    private readonly IUserService _userService;

    public UserLoginViewHandler(ISetupService setupService, IUserService userService)
    {
        _setupService = setupService;
        _userService = userService;
    }

    public async Task<UserLoginViewModel> OnInitAsync()
    {
        var result = InitViewModel();

        var user = await _userService.GetCurrentUser();
        if (user is not null)
        {
            PopulateUserData(result, user);

            result.WishlistUrl = user.WishlistUrl;
        }

        return result;
    }

    public async Task<UserLoginViewModel> SetWishlistUrl(string url)
    {
        var result = InitViewModel();

        var user = await _userService.GetCurrentUser();
        if (user is not null)
        {
            await _userService.UpdateWishlistUrl(user, url);
            PopulateUserData(result, user);
            result.WishlistUrl = url;
        }
        else
            result.ErrorMessage = "NotLoggedIn";

        return result;
    }

    private void PopulateUserData(UserLoginViewModel result, User user)
    {
        result.User = new UserLoginViewModel.CurrentUser()
        {
            Name = user.Name,
            AvatarId = user.AvatarId,
            DiscordTagId = user.DiscordId,
            UserId = user.UserId
        };
    }

    private UserLoginViewModel InitViewModel()
    {
        var result = new UserLoginViewModel();

        result.Title = _setupService.GetTitle();
        return result;
    }
}