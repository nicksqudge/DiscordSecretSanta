using System.Text;
using DiscordSecretSanta.Permissions;

namespace DiscordSecretSanta.Commands;

public class DrawCommand : AbstractCommand<DrawCommand.Input, DrawCommand.Output>
{
    private readonly ICanStartDraw CanStartDraw;
    public DrawCommand(IDataStore dataStore, IMessages messages, ICanStartDraw canStartDraw) : base(dataStore, messages)
    {
        CanStartDraw = canStartDraw;
        AllowedStatuses = [ CampaignStatusId.Open ];
    }

    public sealed record Input(InputUser RequestingUser) : ICommandInput;

    public sealed record Output : ICommandOutput
    {
        public sealed record DirectMessage(DiscordUserId TargetUserId, DiscordUserId SecretSantaId, Uri WishlistUrl);
        public StringBuilder Reply { get; set; } = null!;

        public DirectMessage[] DirectMessages { get; set; } = [];
    }

    protected override async Task<Output> HandleAction(Input input, CancellationToken cancellationToken)
    {
        if (!await CanStartDraw.Can(input.RequestingUser, cancellationToken))
            return Fail(Messages.YouDoNotHavePermissionToDraw());

        if (await DataStore.GetNumberOfMembers(cancellationToken) < 3)
            return Fail(Messages.CouldNotDraw());
        
        if (await DataStore.GetStatus(cancellationToken) != CampaignStatusId.Open)
            return Fail(Messages.CouldNotDraw());

        var directMessages = await DrawSecretSantas(cancellationToken);
        await DataStore.SetStatus(CampaignStatusId.Drawn, cancellationToken);
        return new Output()
        {
            Reply = new StringBuilder().AppendLine(Messages.DrawComplete()),
            DirectMessages = directMessages.ToArray()
        };
    }

    private async Task<List<Output.DirectMessage>> DrawSecretSantas(CancellationToken cancellationToken)
    {
        var members = await DataStore.GetMembers(cancellationToken);
        var unpickedMembers = ShuffledList(members);
        var result = new List<Output.DirectMessage>();

        foreach (var member in members)
        {
            if (unpickedMembers.All(u => u == member.UserId))
                break;
            
            var secretSantaId = unpickedMembers.First(u => u != member.UserId);
            var secretSanta = members.FirstOrDefault(m => m.UserId == secretSantaId);
            ArgumentNullException.ThrowIfNull(secretSanta);
            unpickedMembers.Remove(secretSantaId);
            
            await DataStore.SetSecretSanta(member.UserId, secretSantaId, cancellationToken);
            result.Add(new Output.DirectMessage(member.UserId, secretSantaId, secretSanta.WishlistUrl));
        }

        return result;
    }

    private List<DiscordUserId> ShuffledList(SecretSantaMember[] members)
    {
        var random = new Random();
        var unpickedMembers = members.Select(x => x.UserId).ToArray();
        random.Shuffle(unpickedMembers);
        
        return unpickedMembers.ToList();
    }

    private Output Fail(string message)
        => new ()
        {
            Reply = new StringBuilder().AppendLine(message)
        };
}