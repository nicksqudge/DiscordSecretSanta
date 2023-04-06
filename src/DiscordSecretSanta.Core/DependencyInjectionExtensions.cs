using DiscordSecretSanta.Core.AccessCheck;
using DiscordSecretSanta.Core.AuthProvider;
using DiscordSecretSanta.Core.ViewModels.AdminUsers;
using DiscordSecretSanta.Core.ViewModels.UserLogin;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Core;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDiscordSecretSantaCore(this IServiceCollection services)
    {
        return services
            .AddTransient<IUserLoginViewHandler, UserLoginViewHandler>()
            .AddTransient<IAdminUsersViewHandler, AdminUsersViewHandler>()
            .AddTransient<IAccessCheck, AccessCheck.AccessCheck>();
    }

    public static IServiceCollection AddAuthProviderService<T>(this IServiceCollection services)
        where T : class, IAuthProviderService
    {
        return services.AddTransient<IAuthProviderService, T>();
    }
    
    public static IServiceCollection AddSetupService<T>(this IServiceCollection services)
        where T : class, ISetupService
    {
        return services.AddTransient<ISetupService, T>();
    }
}