using System.Text;

namespace DiscordSecretSanta.Commands;

#pragma warning disable CS9113 // Parameter is unread.
public class SentCommand(IDataStore dataStore, IMessages messages)
#pragma warning restore CS9113 // Parameter is unread.
{
    public async Task<StringBuilder> Handle(CancellationToken cancellationToken)
    {
        return new StringBuilder("");
    }
}