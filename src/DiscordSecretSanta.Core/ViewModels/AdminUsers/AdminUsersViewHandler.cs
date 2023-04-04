using DiscordSecretSanta.Core.Repositories;

namespace DiscordSecretSanta.Core.ViewModels.AdminUsers;

public interface IAdminUsersViewHandler
{
    Task<AdminUsersViewModel> OnInitAsync(CancellationToken cancellationToken);
}

public class AdminUsersViewHandler : IAdminUsersViewHandler
{
    private readonly IUserRepository _userRepository;
    
    public async Task<AdminUsersViewModel> OnInitAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.ListUsers(cancellationToken);
        
        return new AdminUsersViewModel()
        {
            Users = users.Select(u => new UserViewModel(u)).ToList()
        };
    }
}