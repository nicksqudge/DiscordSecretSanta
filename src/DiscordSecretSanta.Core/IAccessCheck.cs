using DiscordSecretSanta.Core.Repositories;

namespace DiscordSecretSanta.Core;

public interface IAccessCheck
{
    Task<bool> CanAccess(AccessCheckInput checksInput, CancellationToken cancellationToken);
}

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

public class AccessCheckInput
{
    public bool MustBeLoggedIn { get; set; }
    public bool MustHaveUserAccount { get; set; }
    public bool MustBeAdmin { get; set; }
}

public class AccessCheckInputBuilder
{
    private AccessCheckInput _result = new();
    
    public AccessCheckInputBuilder MustBeLoggedIn()
    {
        _result.MustBeLoggedIn = true;
        return this;
    }

    public AccessCheckInputBuilder MustHaveUser()
    {
        MustBeLoggedIn();
        _result.MustHaveUserAccount = true;
        return this;
    }

    public AccessCheckInputBuilder MustBeAdmin()
    {
        MustBeLoggedIn();
        MustHaveUser();
        _result.MustBeAdmin = true;
        return this;
    }

    public AccessCheckInput Build()
    {
        return _result;
    }
}