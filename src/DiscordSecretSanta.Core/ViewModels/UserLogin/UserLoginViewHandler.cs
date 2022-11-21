namespace DiscordSecretSanta.Core.ViewModels.UserLogin;

public interface IUserLoginViewHandler
{
    Task<UserLoginViewModel> OnInitAsync();
}

public class UserLoginViewHandler : IUserLoginViewHandler
{
    private readonly ISetupService _setupService;

    public UserLoginViewHandler(ISetupService setupService)
    {
        _setupService = setupService;
    }

    public async Task<UserLoginViewModel> OnInitAsync()
    {
        var result = new UserLoginViewModel();

        result.Title = _setupService.GetTitle();

        return result;
    }
}