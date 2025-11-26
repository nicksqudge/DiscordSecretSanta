using Discord.Commands;
using DiscordSecretSanta.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta;

public class CommandsModule : ModuleBase
{
    private readonly IServiceProvider _services;

    public CommandsModule(IServiceProvider services)
    {
        _services = services;
    }

    [Command("status")]
    [Summary("Provides a status on secret santa")]
    public async Task StatusAsync()
    {
        var command = _services.GetRequiredService<StatusCommand>();
        var reply = await command.Handle(CancellationToken.None);
        await ReplyAsync(reply);
    }
}