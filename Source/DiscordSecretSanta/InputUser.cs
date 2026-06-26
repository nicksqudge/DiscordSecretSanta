using Discord;
using Discord.WebSocket;

namespace DiscordSecretSanta;

public sealed record InputUser(DiscordUserId Id, string Name, bool IsServerAdmin = false)
{
    public static InputUser From(IGuildUser user) 
        => new(
            new DiscordUserId(user.Id),
            user.DisplayName,
            user.GuildPermissions.Administrator
        );
}