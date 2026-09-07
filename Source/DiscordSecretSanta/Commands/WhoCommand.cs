using System.Text;

namespace DiscordSecretSanta.Commands;

public class WhoCommand : AbstractCommand<WhoCommand.Input, WhoCommand.Output>
{
    public sealed record Input(InputUser RequestingUser) : ICommandInput;

    public sealed record Output : ICommandOutput
    {
        public sealed record DirectMessage(DiscordUserId WhoAskedId, DiscordUserId SecretSantaId, Uri SecretSantaWishlist);
        public StringBuilder Reply { get; set; } = null!;
        public DirectMessage? Who { get; set; } = null;
    }
    

    public WhoCommand(IDataStore dataStore, IMessages messages) : base(dataStore, messages)
    {
        AllowedStatuses = [CampaignStatusId.Drawn];
    }

    protected override async Task<Output> HandleAction(Input input, CancellationToken cancellationToken)
    {
        var status = await DataStore.GetStatus(cancellationToken);
        if (status != CampaignStatusId.Drawn)
        {
            return JustMessage(Messages.CouldNotShowWho());
        }
        
        var requester = await DataStore.GetMember(input.RequestingUser.Id, cancellationToken);
        if (requester == null)
            throw new CommandException($"Could not find member: {input.RequestingUser.Id}");

        if (requester.SecretSantaId is null)
            throw new CommandException($"STATUS IS DRAWN MEMBER DOES NOT HAVE SECRET SANTA: {input.RequestingUser.Id}");
        
        var secretSanta = await DataStore.GetMember(requester.SecretSantaId, cancellationToken);
        if (secretSanta == null)
            throw new CommandException($"COULD NOT FIND MEMBER: {input.RequestingUser.Id}");

        return new Output()
        {
            Reply = new StringBuilder(Messages.CouldShow()),
            Who = new Output.DirectMessage(requester.UserId, secretSanta.UserId, secretSanta.WishlistUrl)
        };
    }

    private Output JustMessage(string message)
        => new()
        {
            Reply = new StringBuilder(message)
        };
}