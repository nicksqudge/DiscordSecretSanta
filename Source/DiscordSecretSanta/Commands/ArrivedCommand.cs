using System.Text;

namespace DiscordSecretSanta.Commands;

public class ArrivedCommand: AbstractCommand<ArrivedCommand.Input, ArrivedCommand.Output>
{
    public ArrivedCommand(IDataStore dataStore, IMessages messages) : base(dataStore, messages)
    {
        AllowedStatuses = [CampaignStatusId.Drawn];
    }

    public sealed record Input(DiscordUserId RequestingUserId) : ICommandInput;
    
    public sealed record Output : ICommandOutput
    {
        public sealed record DirectMessage(DiscordUserId Sender);
        
        public StringBuilder Reply { get; set; } = null!;

        public DirectMessage? DirectMessageTo { get; set; }
    }
    

    protected override async Task<Output> HandleAction(Input input,
        CancellationToken cancellationToken)
    {
        var secretSanta = await DataStore.GetMembersSecretSanta(input.RequestingUserId, cancellationToken);
        if (secretSanta is null)
            throw new CommandException($"UNABLE TO FETCH SECRET SANTA FOR USER ID: {input.RequestingUserId}");

        if (secretSanta.SecretSantaId is null || secretSanta.SecretSantaId != input.RequestingUserId)
            throw new CommandException($"THE FETCHED SECRET SANTA OF {input.RequestingUserId} IS UNEXPECTEDLY {secretSanta.SecretSantaId}");
        
        if (secretSanta.SecretSantaStatus == SecretSantaStatus.Arrived)
            return new()
            {
                Reply = new(Messages.AlreadyArrived()),
                DirectMessageTo = null
            };
        
        await DataStore.SetSecretSantaStatus(secretSanta.UserId, SecretSantaStatus.Arrived, cancellationToken);
        return new()
        {
            Reply = new(Messages.MarkedAsArrived()),
            DirectMessageTo = new (secretSanta.UserId)
        };
    }
}