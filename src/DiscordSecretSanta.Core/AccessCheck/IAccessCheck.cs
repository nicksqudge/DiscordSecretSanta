namespace DiscordSecretSanta.Core.AccessCheck;

public interface IAccessCheck
{
    Task<bool> CanAccess(AccessCheckInput checksInput, CancellationToken cancellationToken);
}