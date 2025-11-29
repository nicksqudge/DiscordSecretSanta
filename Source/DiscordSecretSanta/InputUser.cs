using Discord.WebSocket;

namespace DiscordSecretSanta;

public sealed record InputUser(DiscordUserId Id, string Name, bool IsAdmin = false)
{
    public static InputUser From(SocketGuildUser user)
    {
        return new InputUser(
            new DiscordUserId(user.Id),
            user.DisplayName,
            user.GuildPermissions.Administrator
        );
    }
}