using Discord.Commands;

namespace DiscordSecretSanta;

public class StatusModule : ModuleBase
{
    [Command("status")]
    [Summary("Provides a status on secret santa")]
    public Task StatusAsync()
    {
        return ReplyAsync("TODO:// Put in a status message");
    }
}