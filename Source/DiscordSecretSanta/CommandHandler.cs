using Discord.Commands;
using Discord.WebSocket;

namespace DiscordSecretSanta;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;

    public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
    {
        _client = client;
        _commands = commands;
        _services = services;
    }
    
    public async Task InstallCommandsAsync()
    {
        // Hook the MessageReceived event into our command handler
        _client.MessageReceived += HandleInput;
        
        await _commands.AddModulesAsync(typeof(CommandHandler).Assembly,  _services);
    }

    private async Task HandleInput(SocketMessage msg)
    {
        // Don't process the command if it was a system message
        var message = msg as SocketUserMessage;
        if (message == null) return;

        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;

        if (message.Author.IsBot)
            return;

        if (message.HasCharPrefix('!', ref argPos) && message.HasMentionPrefix(_client.CurrentUser, ref argPos))
        {
            await HandleCommandAsync(message, argPos);
        }
    }
    
    private async Task HandleCommandAsync(SocketUserMessage message, int argPos)
    {
        // Create a WebSocket-based command context based on the message
        var context = new SocketCommandContext(_client, message);

        // Execute the command with the command context we just
        // created, along with the service provider for precondition checks.
        await _commands.ExecuteAsync(
            context: context, 
            argPos: argPos,
            _services);
    }
}