using System.Text;

namespace DiscordSecretSanta.Commands;

public class ToggleAdminCommand : AbstractCommand<ToggleAdminCommand.Input, ToggleAdminCommand.Output>
{
    public sealed record Input(InputUser TargetUser, InputUser RequestingUser) : ICommandInput;

    public sealed record Output : ICommandOutput
    {
        public StringBuilder Reply { get; set; } = null!;
    }
    
    public ToggleAdminCommand(IDataStore dataStore, IMessages messages) : base(dataStore, messages)
    {
    }

    protected override async Task<Output> HandleAction(Input input, CancellationToken cancellationToken)
    {
        Logger.Debug($"{input.RequestingUser.Name} is trying to make {input.TargetUser.Name} an admin");
        
        if (!await IsAdmin(input.RequestingUser, cancellationToken))
        {
            Logger.Debug($"{input.RequestingUser.Name} does not have permission");
            return ReturnMessage(Messages.YouDoNotHavePermissionToMakeAdmin());
        }

        if (input.TargetUser.IsServerAdmin)
        {
            Logger.Debug($"{input.TargetUser.Name} is already an admin");
            return ReturnMessage(Messages.IsGuidAdmin(input.TargetUser.Name));
        }

        var isAdmin = await DataStore.IsAdminInConfig(input.TargetUser.Id, cancellationToken);
        await DataStore.ToggleAdmin(input.TargetUser.Id, !isAdmin, cancellationToken);
        
        if (isAdmin)
            return ReturnMessage(Messages.IsNoLongerAnAdmin(input.TargetUser.Name));
        
        return ReturnMessage(Messages.IsNowAnAdmin(input.TargetUser.Name));
    }

    private async Task<bool> IsAdmin(InputUser user, CancellationToken token)
    {
        if (user.IsServerAdmin)
            return true;

        return await DataStore.IsAdminInConfig(user.Id, token);
    }

    private Output ReturnMessage(string message)
        => new()
        {
            Reply = new StringBuilder(message)
        };
}