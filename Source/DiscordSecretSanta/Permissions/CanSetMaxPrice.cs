namespace DiscordSecretSanta.Permissions;

public interface ICanSetMaxPrice : IPermission
{
    
}

public class CanSetMaxPrice(IDataStore dataStore) : ICanSetMaxPrice
{
    public async Task<bool> Can(InputUser user, CancellationToken cancellationToken)
    {
        if (user.IsServerAdmin)
            return true;

        return await dataStore.IsAdminInConfig(user.Id, cancellationToken);
    }
}