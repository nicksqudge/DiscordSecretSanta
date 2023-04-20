using DiscordSecretSanta.Core.AccessCheck;
using DiscordSecretSanta.Core.Repositories;

namespace DiscordSecretSanta.Core.ViewModels.AdminUsers;

public interface IAdminUsersViewHandler
{
    Task<AdminUsersViewModel> OnInitAsync(CancellationToken cancellationToken);

    Task<AdminUsersViewModel> MakeUserAdmin(AdminUsersViewModel context, UserId userId, CancellationToken cancellationToken);

    Task<AdminUsersViewModel> RemoveUserAdmin(AdminUsersViewModel context, UserId userId,
        CancellationToken cancellationToken);

    Task<AdminUsersViewModel> SetUserWishlistUrl(AdminUsersViewModel context, UserId userId, string wishlistUrl,
        CancellationToken cancellationToken);
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
            Users = users.Select(u => new UserAndSecretSantaViewModel(u)).ToList()
        };
    }

    public async Task<AdminUsersViewModel> MakeUserAdmin(AdminUsersViewModel context, UserId userId, CancellationToken cancellationToken)
    {
        var result = await _userRepository.MakeUserAdmin(userId, cancellationToken);
        if (result.IsFailure)
            context.ErrorMessage = "CouldNotMakeUserAdmin";

        return ModifyUser(context, userId, user => user.IsAdmin = true);
    }

    public async Task<AdminUsersViewModel> RemoveUserAdmin(AdminUsersViewModel context, UserId userId, CancellationToken cancellationToken)
    {
        var result = await _userRepository.RemoveUserAdmin(userId, cancellationToken);
        if (result.IsFailure)
            context.ErrorMessage = "CouldNotRemoveUserAdmin";

        return ModifyUser(context, userId, user => user.IsAdmin = false);
    }

    public async Task<AdminUsersViewModel> SetUserWishlistUrl(AdminUsersViewModel context, UserId userId, string wishlistUrl,
        CancellationToken cancellationToken)
    {
        var result = await _userRepository.SaveUserWishlistUrl(userId, new Uri(wishlistUrl), cancellationToken);
        if (result.IsFailure)
            context.ErrorMessage = "CouldNotUpdateWishlistUrl";

        return ModifyUser(context, userId, user => user.WishlistUrl = wishlistUrl);
    }

    private AdminUsersViewModel NotAuthorised()
    {
        return new AdminUsersViewModel()
        {
            Authorised = false,
            Users = new List<UserAndSecretSantaViewModel>()
        };
    }

    private AdminUsersViewModel ModifyUser(AdminUsersViewModel context, UserId userId, Action<UserViewModel> changeAction)
    {
        var users = context.Users.ToList();
        var user = users.FirstOrDefault(u => u.UserId == userId.Value);
        if (user is not null)
            changeAction.Invoke(user);

        context.Users = users;
        return context;
    }
}