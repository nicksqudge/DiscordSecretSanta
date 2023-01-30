using System.Security.Claims;
using DiscordAuthProvider;
using DiscordSecretSanta.Core;
using FluentResults;
using Microsoft.AspNetCore.Components.Authorization;

namespace DiscordSecretSanta.Blazor.Implementations;

public class UserService : IUserService
{
    private readonly AuthenticationStateProvider _authenticationState;
    private readonly ILogger<UserService> _logger;

    public UserService(AuthenticationStateProvider authenticationState, ILogger<UserService> logger)
    {
        _authenticationState = authenticationState;
        _logger = logger;
    }

    public async Task<User?> GetCurrentUser()
    {
        var authState = await _authenticationState.GetAuthenticationStateAsync();

        if (authState.User is null)
        {
            return null;
        }

        var name = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        if (name is null)
        {
            _logger.LogWarning("Has no claim for name");
            return null;
        }
        
        var discordTag = authState.User.Claims.FirstOrDefault(c => c.Type == DiscordOptions.DiscordTagKey);
        if (discordTag is null)
        {
            _logger.LogWarning("Has no claim for discord tag");
            return null;
        }
        
        var avatar = authState.User.Claims.FirstOrDefault(c => c.Type == DiscordOptions.AvatarKey);
        if (avatar is null)
        {
            _logger.LogWarning("Has no claim for avatar");
            return null;
        }

        var userId = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            _logger.LogWarning("Has no claim for user id");
            return null;
        }

        return new User(
            name.Value,
            discordTag.Value,
            avatar.Value,
            "",
            userId.Value
        );
    }

    public Task<Result> UpdateWishlistUrl(User user, string url)
    {
        throw new NotImplementedException();
    }
}