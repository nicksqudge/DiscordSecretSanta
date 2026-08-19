using System.Text;

namespace DiscordSecretSanta.Commands;

#pragma warning disable CS9113 // Parameter is unread.
public class CloseCommand(IDataStore dataStore, IMessages messages)
#pragma warning restore CS9113 // Parameter is unread.
{
    public Task<StringBuilder> Handle(DiscordUserId requestingUserId, CancellationToken cancellationToken)
    {
        return Task.FromResult(new StringBuilder());
    }
}