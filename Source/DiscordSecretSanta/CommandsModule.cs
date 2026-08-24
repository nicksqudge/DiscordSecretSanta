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

    [RequireContext(ContextType.Guild)]
    [Command("status")]
    [Summary("Provides a status on secret santa")]
    public async Task StatusAsync()
    {
        var command = _services.GetRequiredService<StatusCommand>();
        var reply = await command.Handle(CancellationToken.None);
        await ReplyAsync(reply.ToString());
    }

    [RequireContext(ContextType.Guild)]
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
            var targetUser = InputUser.From(target);
            var command = _services.GetRequiredService<ToggleAdminCommand>();
            var reply = await command.Handle(targetUser, requester, CancellationToken.None);
            await ReplyAsync(reply.ToString());
        });
    }

    [RequireContext(ContextType.Guild)]
    [Command("max-price")]
    [Summary("(Admin Only) Sets the max price for gifts")]
    public async Task SetMaxPriceAsync(string maxPrice)
    {
        await IfUserIsValid(async (requester) =>
        {
            var command = _services.GetRequiredService<SetMaxPriceCommand>();
            var reply = await command.Handle(requester, maxPrice, CancellationToken.None);
            await ReplyAsync(reply.ToString());
        });
    }
    
    [RequireContext(ContextType.Guild)]
    [Command("join")]
    [Summary("When open, allows people to sign up to the secret santa with a wishlist url")]
    public async Task JoinAsync(string wishlistUrl)
    {
        await IfUserIsValid(async (requester) =>
        {
            var command = _services.GetRequiredService<JoinCommand>();
            var reply = await command.Handle(requester.Id, wishlistUrl, CancellationToken.None);
            await ReplyAsync(reply.ToString());
        });
    }

    [RequireContext(ContextType.Guild)]
    [Command("draw")]
    [Summary("(Admin Only) Draws the secret santas")]
    public async Task DrawSecretSantaAsync()
    {
        await IfUserIsValid(async (requester) =>
        {
            var command = _services.GetRequiredService<DrawCommand>();
            var messages = _services.GetRequiredService<IMessages>();
            var output = await command.Handle(new DrawCommand.Input(requester), CancellationToken.None);
            if (output.DirectMessages.Length != 0)
            {
                foreach (var dm in output.DirectMessages)
                    await SendDirectMessage(dm, messages);
            }
            
            await ReplyAsync(output.Reply.ToString());
        });
    }
    
    [Command("who")]
    [Summary("Find out who you have drawn for Secret Santa")]
    public async Task WhoAsync()
    {
        await IfUserIsValid(async (requester) =>
        {
            var command = _services.GetRequiredService<WhoCommand>();
            var messages = _services.GetRequiredService<IMessages>();
            var (reply, directMessage) = await command.Handle(requester, CancellationToken.None);
            if (directMessage != null)
                await SendDirectMessage(directMessage, messages);
            
            await ReplyAsync(reply.ToString());
        });
    }

    [RequireContext(ContextType.DM)]
    [Command("sent")]
    [Summary("Tell the person you are buying for that their gift is in the way")]
    public async Task SentAsync()
    {
        await IfUserIsValid(async (requester) =>
        {
            var command = _services.GetRequiredService<SentCommand>();
            var messages = _services.GetRequiredService<IMessages>();
            var (reply, directMessage) = await command.Handle(requester.Id, CancellationToken.None);
            if (directMessage != null)
                await SendDirectMessage(directMessage, messages);
            
            await ReplyAsync(reply.ToString());
        });
    }
    
    [RequireContext(ContextType.DM)]
    [Command("arrived")]
    [Summary("Tell your secret santa that your gift has arrived")]
    public async Task ArrivedAsync()
    {
        await IfUserIsValid(async (requester) =>
        {
            var command = _services.GetRequiredService<ArrivedCommand>();
            var messages = _services.GetRequiredService<IMessages>();
            var response = await command.Handle(new ArrivedCommand.Input(requester.Id), CancellationToken.None);
            if (response.DirectMessageTo != null)
                await SendDirectMessage(response.DirectMessageTo, messages);
            
            await ReplyAsync(response.Reply.ToString());
        });
    }

    private async Task IfUserIsValid(Func<InputUser, Task> action)
    {
        if (Context.User.IsBot)
        {
            Logger.Debug("User is a bot");
            return;
        }

        if (Context.User is SocketGuildUser requester)
        {
            await action(InputUser.From(requester));
            return;
        }

        await action(new InputUser(new DiscordUserId(Context.User.Id), Context.User.GlobalName));
    }

    private async Task SendDirectMessage(DrawCommand.Output.DirectMessage dm, IMessages message)
    {
        var recipient = await GetGuildUser(dm.TargetUserId.Value);
        if (recipient is null)
        {
            Logger.Error($"Could not find user {dm.TargetUserId.Value}");
            return;
        }
        
        var secretSanta = await GetGuildUser(dm.SecretSantaId.Value);
        if (secretSanta is null)
        {
            Logger.Error($"Could not find user {dm.SecretSantaId.Value}");
            return;
        }

        var channel = await recipient.CreateDMChannelAsync();
        await channel.SendMessageAsync(message.SecretSantaDrawnDirectMessage(Context.Guild.Name, secretSanta.DisplayName, dm.WishlistUrl));
    }

    private async Task SendDirectMessage(WhoCommand.DirectMessage dm, IMessages message)
    {
        var recipient = await GetGuildUser(dm.WhoAskedId.Value);
        if (recipient is null)
        {
            Logger.Error($"Could not find user {dm.WhoAskedId.Value}");
            return;
        }
        
        var secretSanta = await GetGuildUser(dm.SecretSantaId.Value);
        if (secretSanta is null)
        {
            Logger.Error($"Could not find user {dm.SecretSantaId.Value}");
            return;
        }

        var channel = await recipient.CreateDMChannelAsync();
        await channel.SendMessageAsync(message.SecretSantaDrawnDirectMessage(Context.Guild.Name, secretSanta.DisplayName, dm.SecretSantaWishlist));
    }

    private async Task SendDirectMessage(SentCommand.DirectMessage dm, IMessages message)
    {
        var secretSanta = await GetGuildUser(dm.Receiver.Value);
        if (secretSanta is null)
        {
            Logger.Error($"Could not find user {dm.Receiver.Value}");
            return;
        }
        
        var channel = await secretSanta.CreateDMChannelAsync();
        await channel.SendMessageAsync(message.YourGiftIsOnTheWay());
    }
    
    private async Task SendDirectMessage(ArrivedCommand.Output.DirectMessage dm, IMessages message)
    {
        var secretSanta = await GetGuildUser(dm.Sender.Value);
        if (secretSanta is null)
        {
            Logger.Error($"Could not find user {dm.Sender.Value}");
            return;
        }
        
        var channel = await secretSanta.CreateDMChannelAsync();
        await channel.SendMessageAsync(message.YourGiftHasArrived());
    }

    private async Task<IGuildUser?> GetGuildUser(ulong id)
    {
        if (Context.Guild is not null)
            return await Context.Guild.GetUserAsync(id);
        
        if (Context.User is SocketUser user)
        {
            var guild = user.MutualGuilds.First();
            return guild.GetUser(id);
        }

        return null;
    }
}