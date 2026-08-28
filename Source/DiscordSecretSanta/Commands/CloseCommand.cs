using System.Text;
using DiscordSecretSanta.Permissions;

namespace DiscordSecretSanta.Commands;

public class CloseCommand : AbstractCommand<CloseCommand.Input, CloseCommand.Output>
{
    public sealed record Input(InputUser RequestingUser) : ICommandInput;

    public sealed record Output : ICommandOutput
    {
        public StringBuilder Reply { get; set; } = null!;
    }

    private readonly ICanClose _permission;

    public CloseCommand(IDataStore dataStore, IMessages messages, ICanClose permission) : base(dataStore, messages)
    {
        _permission = permission;
        AllowedStatuses = [CampaignStatusId.Drawn, CampaignStatusId.Open];
    }

    protected override async Task<Output> HandleAction(Input input, CancellationToken cancellationToken)
    {
        if (!await _permission.Can(input.RequestingUser, cancellationToken))
            return ReturnMessage(Messages.YouAreNotAnAdmin());

        await DataStore.SetStatus(CampaignStatusId.Closed, cancellationToken);
        return ReturnMessage(Messages.CampaignClosed());
    }

    private Output ReturnMessage(string message)
        => new ()
        {
            Reply = new StringBuilder(message)
        };
}