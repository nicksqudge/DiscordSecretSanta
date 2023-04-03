using System.Runtime.CompilerServices;
using DiscordSecretSanta.Core.ViewModels.UserLogin;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Core;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddTransient<IUserLoginViewHandler, UserLoginViewHandler>();
        return services;
    }

    public static IServiceCollection AddUserService<T>(this IServiceCollection services)
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