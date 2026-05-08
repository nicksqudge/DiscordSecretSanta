namespace DiscordSecretSanta.Permissions;

public interface ICanStartDraw : IPermission
{
    
}

public class CanStartDraw : ICanStartDraw
{
    public Task<bool> Can(InputUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.IsServerAdmin);
}