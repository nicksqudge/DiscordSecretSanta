using System.Text;

namespace DiscordSecretSanta.Commands;

public class WhoCommand(IDataStore dataStore, IMessages messages)
{
    public sealed record DirectMessage(DiscordUserId targetUserId, DiscordUserId secretSanta);
    
    public async Task<(StringBuilder Response, DirectMessage? DirectMessages)> Handle(InputUser requestingUser,
        CancellationToken cancellationToken)
    {
        var status = await dataStore.GetStatus(cancellationToken);
        if (status != Status.Drawn)
        {
            return (new StringBuilder().AppendLine(messages.CouldNotShowWho()), null);
        }
        
        return (new StringBuilder(), new DirectMessage(new DiscordUserId(12308123), new DiscordUserId(12308123)));
    }
}