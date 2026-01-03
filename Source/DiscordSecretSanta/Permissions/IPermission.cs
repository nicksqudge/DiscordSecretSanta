namespace DiscordSecretSanta.Permissions;

public interface IPermission
{
    Task<bool> Can(InputUser user, CancellationToken cancellationToken);
}