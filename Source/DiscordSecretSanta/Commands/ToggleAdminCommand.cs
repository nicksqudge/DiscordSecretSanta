using System.Text;

namespace DiscordSecretSanta.Commands;

public class ToggleAdminCommand
{
    private readonly IDataStore _dataStore;
    private readonly IMessages _messages;

    public ToggleAdminCommand(IDataStore dataStore, IMessages messages)
    {
        _dataStore = dataStore;
        _messages = messages;
    }

    public async Task<StringBuilder> Handle(InputUser targetUser, InputUser requestingUser, CancellationToken token)
    {
        Logger.Debug($"{requestingUser.Name} is trying to make {targetUser.Name} an admin");
        
        var result = new StringBuilder();
        if (!await IsAdmin(requestingUser, token))
        {
            Logger.Debug($"{requestingUser.Name} does not have permission");
            return result.AppendLine(_messages.YouDoNotHavePermissionToMakeAdmin());
        }

        if (targetUser.IsAdmin)
        {
            Logger.Debug($"{targetUser.Name} is already an admin");
            return result.AppendLine(_messages.IsGuidAdmin(targetUser.Name));
        }

        var isAdmin = await _dataStore.IsAdmin(targetUser.Id, token);
        await _dataStore.ToggleAdmin(targetUser.Id, !isAdmin, token);
        
        if (isAdmin)
            return result.AppendLine(_messages.IsNoLongerAnAdmin(targetUser.Name));
        
        return result.AppendLine(_messages.IsNowAnAdmin(targetUser.Name));
    }

    private async Task<bool> IsAdmin(InputUser user, CancellationToken token)
    {
        if (user.IsAdmin)
            return true;

        return await _dataStore.IsAdmin(user.Id, token);
    }
}