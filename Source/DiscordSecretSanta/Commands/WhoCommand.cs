using System.Text;

namespace DiscordSecretSanta.Commands;

public class WhoCommand
{
    public sealed record DirectMessage(DiscordUserId targetUserId, DiscordUserId secretSanta);
    
    public async Task<(StringBuilder Response, DirectMessage DirectMessages)> Handle(InputUser requestingUser,
        CancellationToken cancellationToken)
    {
        return (new StringBuilder(), new DirectMessage(new DiscordUserId(12308123), new DiscordUserId(12308123)));
    }
}