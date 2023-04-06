using DiscordSecretSanta.Core.AuthProvider;
using DiscordSecretSanta.Core.Repositories;

namespace DiscordSecretSanta.Core.AccessCheck;

public class AccessCheck : IAccessCheck
{
    private readonly IAuthProviderService _authProviderService;
    private readonly IUserRepository _userRepository;

    public AccessCheck(IUserRepository userRepository, IAuthProviderService authProviderService)
    {
        _userRepository = userRepository;
        _authProviderService = authProviderService;
    }

    public async Task<bool> CanAccess(AccessCheckInput checksInput, CancellationToken cancellationToken)
    {
        if (checksInput.MustBeLoggedIn == false)
            return true;

        var authUser = await _authProviderService.GetCurrentUser(cancellationToken);
        if (authUser.HasNoValue)
            return false;

        var user = await _userRepository.GetUser(authUser.Value.UserId, cancellationToken);
        if (checksInput.MustHaveUserAccount && user.HasNoValue)
            return false;

        if (checksInput.MustBeAdmin && user.Value.IsAdmin == false)
            return false;

        return true;
    }
}