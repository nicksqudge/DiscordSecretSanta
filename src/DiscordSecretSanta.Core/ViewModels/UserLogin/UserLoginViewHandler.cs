using CSharpFunctionalExtensions;
using DiscordSecretSanta.Core.Repositories;

namespace DiscordSecretSanta.Core.ViewModels.UserLogin;

public interface IUserLoginViewHandler
{
    Task<UserLoginViewModel> OnInitAsync(CancellationToken cancellationToken);
    Task<UserLoginViewModel> SetWishlistUrl(string url, CancellationToken cancellationToken);
}

public class UserLoginViewHandler : IUserLoginViewHandler
{
    private readonly ISetupService _setupService;
    private readonly IAuthProviderService _authProviderService;
    private readonly IUserRepository _userRepository;

    public UserLoginViewHandler(ISetupService setupService, IAuthProviderService authProviderService, IUserRepository userRepository)
    {
        _setupService = setupService;
        _authProviderService = authProviderService;
        _userRepository = userRepository;
    }

    public async Task<UserLoginViewModel> OnInitAsync(CancellationToken cancellationToken)
    {
        var result = InitViewModel();

        var user = await GetCurrentUser(cancellationToken);
        if (user.HasValue)
            PopulateUserData(result, user.Value);

        return result;
    }

    public async Task<UserLoginViewModel> SetWishlistUrl(string url, CancellationToken cancellationToken)
    {
        var result = InitViewModel();
        
        var user = await GetCurrentUser(cancellationToken);
        if (user.HasValue)
        {
            PopulateUserData(result, user.Value);

            var updateResult = await _userRepository.SaveUserWishlistUrl(
                new UserId(result.User!.UserId),
                new Uri(url, UriKind.Absolute), 
                cancellationToken);
            
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

    private async Task<Maybe<User>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var (hasUser, currentUser) = await _authProviderService.GetCurrentUser(cancellationToken);

        if (!hasUser)
            return Maybe<User>.None;

        if (await _userRepository.DoesUserExist(currentUser.UserId, cancellationToken))
            return await _userRepository.GetUser(currentUser.UserId, cancellationToken);

        var create = await _userRepository.CreateUser(
            new User(currentUser.Name, currentUser.DiscordId, currentUser.AvatarId, currentUser.UserId),
            cancellationToken);

        if (create.IsSuccess)
        {
            var userCount = await _userRepository.CountUsers(cancellationToken);
            if (userCount <= 1)
            {
                await _userRepository.MakeUserAdmin(create.Value.UserId, cancellationToken);
                create.Value.IsAdmin = true;
            }

            return create.Value;
        }

        return Maybe<User>.None;
    }

    private void PopulateUserData(UserLoginViewModel result, User user)
    {
        result.User = new UserLoginViewModel.CurrentUser()
        {
            Name = user.Name,
            AvatarId = user.AvatarId,
            DiscordTagId = user.DiscordId,
            UserId = user.UserId.Value,
            IsAdmin = user.IsAdmin
        };

        result.WishlistUrl = user.WishlistUrl?.ToString() ?? string.Empty;
    }

    private UserLoginViewModel InitViewModel()
    {
        var result = new UserLoginViewModel();

        result.Title = _setupService.GetTitle();
        return result;
    }
}