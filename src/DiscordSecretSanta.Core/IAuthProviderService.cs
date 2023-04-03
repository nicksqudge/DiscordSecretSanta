using CSharpFunctionalExtensions;

namespace DiscordSecretSanta.Core;

public record AuthenticatedUser(string Name, string DiscordId, string AvatarId, UserId UserId);

public interface IAuthProviderService
{
    Task<Maybe<AuthenticatedUser>> GetCurrentUser(CancellationToken cancellationToken);
}