using System.Security.Claims;
using CSharpFunctionalExtensions;
using DiscordAuthProvider;
using DiscordSecretSanta.Core;
using DiscordSecretSanta.Core.AuthProvider;
using DiscordSecretSanta.Core.Repositories;
using Microsoft.AspNetCore.Components.Authorization;

namespace DiscordSecretSanta.Blazor.Implementations;

public class AuthProviderService : IAuthProviderService
{
    private readonly AuthenticationStateProvider _authenticationState;
    private readonly ILogger<AuthProviderService> _logger;

    public AuthProviderService(AuthenticationStateProvider authenticationState, ILogger<AuthProviderService> logger, IUserRepository userRepository)
    {
        _authenticationState = authenticationState;
        _logger = logger;
    }

    public async Task<Maybe<AuthenticatedUser>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var authState = await _authenticationState.GetAuthenticationStateAsync();

        if (authState.User is null)
        {
            return Maybe<AuthenticatedUser>.None;
        }

        var name = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        if (name is null)
        {
            _logger.LogWarning("Has no claim for name");
            return Maybe<AuthenticatedUser>.None;
        }
        
        var discordTag = authState.User.Claims.FirstOrDefault(c => c.Type == DiscordOptions.DiscordTagKey);
        if (discordTag is null)
        {
            _logger.LogWarning("Has no claim for discord tag");
            return Maybe<AuthenticatedUser>.None;
        }
        
        var avatar = authState.User.Claims.FirstOrDefault(c => c.Type == DiscordOptions.AvatarKey);
        if (avatar is null)
        {
            _logger.LogWarning("Has no claim for avatar");
            return Maybe<AuthenticatedUser>.None;
        }

        var userId = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            _logger.LogWarning("Has no claim for user id");
            return Maybe<AuthenticatedUser>.None;
        }

        var result = new AuthenticatedUser(
            name.Value,
            discordTag.Value,
            avatar.Value,
            new UserId(userId.Value)
        );

        return result;
    }
}