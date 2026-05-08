using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
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
        await ReplyAsync(reply.ToString());
    }

    [Command("open")]
    [Summary("(Admin Only) Opens the secret santa for sign ups")]
    public async Task OpenAsync()
    {
        var command = _services.GetRequiredService<OpenCommand>();
        var reply = await command.Handle(CancellationToken.None);
        await ReplyAsync(reply.ToString());
    }

    [Command("add")]
    [Summary("(Admin Only) Adds an admin to be able to manage secret santa")]
    public async Task AddAdminAsync(SocketGuildUser target)
    {
        await IfUserIsValid(async (requester) =>
        {
            var requestingUser = InputUser.From(requester);
            var targetUser = InputUser.From(target);
            var command = _services.GetRequiredService<ToggleAdminCommand>();
            var reply = await command.Handle(targetUser, requestingUser, CancellationToken.None);
            await ReplyAsync(reply.ToString());
        });
    }

    [Command("max-price")]
    [Summary("(Admin Only) Sets the max price for gifts")]
    public async Task SetMaxPriceAsync(string maxPrice)
    {
        await IfUserIsValid(async (requester) =>
        {
            var requestingUser = InputUser.From(requester);
            var command = _services.GetRequiredService<SetMaxPriceCommand>();
            var reply = await command.Handle(requestingUser, maxPrice, CancellationToken.None);
            await ReplyAsync(reply.ToString());
        });
    }
    
    [Command("join")]
    [Summary("When open, allows people to sign up to the secret santa with a wishlist url")]
    public async Task JoinAsync(string wishlistUrl)
    {
        await IfUserIsValid(async (requester) =>
        {
            var requestingUser = InputUser.From(requester);
            var command = _services.GetRequiredService<JoinCommand>();
            var reply = await command.Handle(requestingUser.Id, wishlistUrl, CancellationToken.None);
            await ReplyAsync(reply.ToString());
        });
    }

    [Command("draw")]
    [Summary("(Admin Only) Draws the secret santas")]
    public async Task DrawSecretSantaAsync()
    {
        await IfUserIsValid(async (requester) =>
        {
            var requestingUser = InputUser.From(requester);
            var command = _services.GetRequiredService<DrawCommand>();
            var messages = _services.GetRequiredService<IMessages>();
            var (reply, directMessages) = await command.Handle(requestingUser, CancellationToken.None);
            if (directMessages.Length != 0)
            {
                foreach (var dm in directMessages)
                    await SendDirectMessage(dm, messages);
            }
            
            await ReplyAsync(reply.ToString());
        });
    }

    private async Task IfUserIsValid(Func<SocketGuildUser, Task> action)
    {
        if (Context.User is not SocketGuildUser requester)
        {
            Logger.Debug("Not a guild user");
            return;
        }
        
        await action(requester);
    }

    private async Task SendDirectMessage(DrawCommand.DirectMessage dm, IMessages message)
    {
        var recipient = await Context.Guild.GetUserAsync(dm.TargetUserId.Value);
        var secretSanta = await Context.Guild.GetUserAsync(dm.SecretSantaId.Value);

        var channel = await recipient.CreateDMChannelAsync();
        await channel.SendMessageAsync(message.SecretSantaDrawnDirectMessage(Context.Guild.Name, secretSanta.DisplayName, dm.WishlistUrl));
    }
}