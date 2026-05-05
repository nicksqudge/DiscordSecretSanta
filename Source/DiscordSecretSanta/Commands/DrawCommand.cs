using System.Text;
using DiscordSecretSanta.Permissions;

namespace DiscordSecretSanta.Commands;

public class DrawCommand(IDataStore dataStore, IMessages messages, ICanStartDraw canStartDraw)
{
    public sealed record DirectMessage(DiscordUserId TargetUserId, DiscordUserId SecretSantaId, Uri WishlistUrl);

    public async Task<(StringBuilder Response, DirectMessage[] DirectMessages)> Handle(InputUser requestingUser, CancellationToken cancellationToken)
    {
        if (!await canStartDraw.Can(requestingUser, cancellationToken))
            return Fail(messages.YouDoNotHavePermissionToDraw());

        if (await dataStore.GetNumberOfMembers(cancellationToken) < 3)
            return Fail(messages.CouldNotDraw());
        
        if (await dataStore.GetStatus(cancellationToken) != Status.Open)
            return Fail(messages.CouldNotDraw());

        var directMessages = await DrawSecretSantas(cancellationToken);
        await dataStore.SetStatus(Status.Drawn, cancellationToken);
        return (new StringBuilder().AppendLine(messages.DrawComplete()), directMessages.ToArray());
    }

    private async Task<List<DirectMessage>> DrawSecretSantas(CancellationToken cancellationToken)
    {
        var members = await dataStore.GetMembers(cancellationToken);
        var unpickedMembers = ShuffledList(members);
        var result = new List<DirectMessage>();

        foreach (var member in members)
        {
            var secretSantaId = unpickedMembers.First(u => u != member.UserId);
            var secretSanta = members.FirstOrDefault(m => m.UserId == secretSantaId);
            ArgumentNullException.ThrowIfNull(secretSanta);
            unpickedMembers.Remove(secretSantaId);
            
            await dataStore.SetSecretSanta(member.UserId, secretSantaId, cancellationToken);
            result.Add(new DirectMessage(member.UserId, secretSantaId, secretSanta.WishlistUrl));
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

    private (StringBuilder Response, DirectMessage[] DirectMessages) Fail(string message)
        => (new StringBuilder().AppendLine(message), new List<DirectMessage>().ToArray());
}