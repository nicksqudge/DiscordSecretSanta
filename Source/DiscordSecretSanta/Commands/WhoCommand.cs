using System.Text;

namespace DiscordSecretSanta.Commands;

public class WhoCommand(IDataStore dataStore, IMessages messages)
{
    public sealed record DirectMessage(DiscordUserId WhoAskedId, DiscordUserId SecretSantaId, Uri SecretSantaWishlist);
    
    public async Task<(StringBuilder Response, DirectMessage? DirectMessages)> Handle(InputUser requestingUser,
        CancellationToken cancellationToken)
    {
        var status = await dataStore.GetStatus(cancellationToken);
        if (status != Status.Drawn)
        {
            return JustMessage(messages.CouldNotShowWho());
        }
        
        var requester = await dataStore.GetMember(requestingUser.Id, cancellationToken);
        if (requester == null)
            return UnexpectedError($"COULD NOT FIND MEMBER: {requestingUser.Id}");

        if (requester.SecretSantaId is null)
            return UnexpectedError($"STATUS IS DRAWN MEMBER DOES NOT HAVE SECRET SANTA: {requestingUser.Id}");
        
        var secretSanta = await dataStore.GetMember(requester.SecretSantaId, cancellationToken);
        if (secretSanta == null)
            return UnexpectedError($"COULD NOT FIND MEMBER: {requestingUser.Id}");
        
        return (new StringBuilder().AppendLine(messages.CouldShow()), new DirectMessage(requester.UserId, secretSanta.UserId, secretSanta.WishlistUrl));
    }

    private (StringBuilder Response, DirectMessage? DirectMessages) JustMessage(string message)
    {
        return (new StringBuilder().AppendLine(message), null);
    }
    
    private (StringBuilder Response, DirectMessage? DirectMessages) UnexpectedError(string message)
        => JustMessage(messages.UnexpectedError(nameof(WhoCommand), message));   
}