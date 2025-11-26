using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta;

public static class SecretSantaBot
{
    public static async Task RunAsync(IServiceProvider services)
    {
        var config = services.GetRequiredService<Configuration>();
        var client = services.GetRequiredService<DiscordSocketClient>();
        client.Log += Logger.Log;

        var commandHandler = services.GetRequiredService<CommandHandler>();
        await commandHandler.InstallCommandsAsync();
        
        await client.LoginAsync(TokenType.Bot, config.Token);
        await client.StartAsync();
        
        await Task.Delay(-1);
    }
}