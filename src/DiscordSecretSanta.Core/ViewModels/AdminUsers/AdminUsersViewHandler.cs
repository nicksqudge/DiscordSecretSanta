using DiscordSecretSanta.Core.Repositories;

namespace DiscordSecretSanta.Core.ViewModels.AdminUsers;

public interface IAdminUsersViewHandler
{
    Task<AdminUsersViewModel> OnInitAsync(CancellationToken cancellationToken);
}

public class AdminUsersViewHandler : IAdminUsersViewHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IAccessCheck _accessCheck;

    public AdminUsersViewHandler(IUserRepository userRepository, IAccessCheck accessCheck)
    {
        _userRepository = userRepository;
        _accessCheck = accessCheck;
    }

    public async Task<AdminUsersViewModel> OnInitAsync(CancellationToken cancellationToken)
    {
        var isAuthorised = await _accessCheck.CanAccess(new AccessCheckInputBuilder()
            .MustHaveUser()
            .MustBeLoggedIn()
            .MustBeAdmin()
            .Build(), cancellationToken);
        if (isAuthorised == false)
            return NotAuthorised();
        
        var users = await _userRepository.ListUsers(cancellationToken);
        
        return new AdminUsersViewModel()
        {
            Authorised = true,
            Users = users.Select(u => new UserViewModel(u)).ToList()
        };
    }

    private AdminUsersViewModel NotAuthorised()
    {
        return new AdminUsersViewModel()
        {
            Authorised = false,
            Users = new List<UserViewModel>()
        };
    }
}