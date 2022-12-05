namespace DiscordSecretSanta.Core.ViewModels.UserLogin;

public interface IUserLoginViewHandler
{
    Task<UserLoginViewModel> OnInitAsync();
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
        var result = new UserLoginViewModel();

        result.Title = _setupService.GetTitle();

        var user = await _userService.GetCurrentUser();
        if (user is not null)
        {
            result.User = new UserLoginViewModel.CurrentUser()
            {
                Name = user.Name,
                AvatarId = user.AvatarId,
                DiscordTagId = user.DiscordId
            };

            result.WishlistUrl = user.WishlistUrl;
        }

        return result;
    }
}