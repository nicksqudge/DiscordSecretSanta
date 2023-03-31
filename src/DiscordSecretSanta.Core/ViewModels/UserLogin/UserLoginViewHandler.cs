namespace DiscordSecretSanta.Core.ViewModels.UserLogin;

public interface IUserLoginViewHandler
{
    Task<UserLoginViewModel> OnInitAsync(CancellationToken cancellationToken);
    Task<UserLoginViewModel> SetWishlistUrl(string url, CancellationToken cancellationToken);
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

    public async Task<UserLoginViewModel> OnInitAsync(CancellationToken cancellationToken)
    {
        var result = InitViewModel();

        var user = await _userService.GetCurrentUser(cancellationToken);
        if (user is not null)
        {
            PopulateUserData(result, user);

            result.WishlistUrl = user.WishlistUrl.ToString();
        }

        return result;
    }

    public async Task<UserLoginViewModel> SetWishlistUrl(string url, CancellationToken cancellationToken)
    {
        var result = InitViewModel();

        if (result.HasUser)
        {
            var updateResult = await _userService.UpdateWishlistUrl(result.User!.UserId, new Uri(url, UriKind.Absolute), cancellationToken);
            if (updateResult.IsSuccess)
                result.WishlistUrl = url;
            else
                result.ErrorMessage = "UnableToUpdate_User";
        }
        else
        {
            result.ErrorMessage = "NotLoggedIn";    
        }

        return result;
    }

    private void PopulateUserData(UserLoginViewModel result, User user)
    {
        result.User = new UserLoginViewModel.CurrentUser()
        {
            Name = user.Name,
            AvatarId = user.AvatarId,
            DiscordTagId = user.DiscordId,
            UserId = user.UserId.Value
        };
    }

    private UserLoginViewModel InitViewModel()
    {
        var result = new UserLoginViewModel();

        result.Title = _setupService.GetTitle();
        return result;
    }
}