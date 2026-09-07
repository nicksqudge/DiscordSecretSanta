using System.Text;

namespace DiscordSecretSanta.Commands;

public class SentCommand : AbstractCommand<SentCommand.Input, SentCommand.Output>
{
    public sealed record Input(DiscordUserId RequestingUserId) : ICommandInput;

    public sealed record Output : ICommandOutput
    {
        public sealed record DirectMessage(DiscordUserId Receiver);

        public StringBuilder Reply { get; set; } = null!;
        
        public DirectMessage? ToSend { get; set; }
    }

    public SentCommand(IDataStore dataStore, IMessages messages) : base(dataStore, messages)
    {
        AllowedStatuses = [CampaignStatusId.Drawn];
    }

    protected override async Task<Output> HandleAction(Input input, CancellationToken cancellationToken)
    {
        var status = await DataStore.GetStatus(cancellationToken);
        if (status != CampaignStatusId.Drawn)
            return ReturnFail(Messages.StatusNotValidForSent());
        
        var requester = await DataStore.GetMember(input.RequestingUserId, cancellationToken);
        if (requester is null)
            throw new CommandException($"UNABLE TO FETCH REQUESTING USER ID: {input.RequestingUserId}");

        if (requester.SecretSantaId is null)
            throw new CommandException($"REQUESTING USER ID: {input.RequestingUserId} DOES NOT HAVE AN ASSIGNED SECRET SANTA");

        if (requester.SecretSantaStatus != SecretSantaStatus.Pending && requester.SecretSantaStatus is not null)
            return ReturnFail(Messages.AlreadySent());
        
        await DataStore.SetSecretSantaStatus(requester.UserId, SecretSantaStatus.Sent, cancellationToken);
        var directMessage = new Output.DirectMessage(requester.SecretSantaId);
        return new Output()
        {
            Reply = new StringBuilder(Messages.MarkedAsSent()),
            ToSend = directMessage
        };
    }
    
    private Output ReturnFail(string message)
        => new ()
        {
            Reply = new StringBuilder(message)
        };
}