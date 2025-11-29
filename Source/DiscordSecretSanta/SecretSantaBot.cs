using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta;

public static class SecretSantaBot
{
    public static async Task Run(IServiceProvider services)
    {
        var config = services.GetRequiredService<Configuration>();
        var client = services.GetRequiredService<DiscordSocketClient>();
        client.Log += Logger.Log;

        var commandHandler = services.GetRequiredService<CommandHandler>();
        await commandHandler.InstallCommandsAsync();
        Logger.Debug("Setup finished");
        
        await client.LoginAsync(TokenType.Bot, config.Token);
        Logger.Debug("Logged in");
        await client.StartAsync();
        Logger.Debug("Started");
        
        await Task.Delay(-1);
    }
}