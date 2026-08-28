namespace DiscordSecretSanta.Permissions;

public interface ICanClose : IPermission;

public class CanClose(IDataStore dataStore) : ICanClose
{
    public async Task<bool> Can(InputUser user, CancellationToken cancellationToken)
    {
        if (user.IsServerAdmin)
            return true;

        return await dataStore.IsAdminInConfig(user.Id, cancellationToken);
    }
}