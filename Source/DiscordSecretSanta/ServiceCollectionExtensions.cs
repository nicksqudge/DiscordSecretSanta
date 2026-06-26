using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Permissions;
using DiscordSecretSanta.Translations.English;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta;

public class SecretSantaServices
{
    public IServiceCollection Services { get; private set; }

    public SecretSantaServices(IServiceCollection services)
    {
        Services = services;
    }
}

public static class ServiceCollectionExtensions
{
    public static SecretSantaServices AddDiscordSecretSanta(this IServiceCollection services, string token)
    {
        var config = new Configuration()
        {
            Token = token
        };
        services.AddSingleton(config);

        var discordConfig = new DiscordSocketConfig()
        {
            AlwaysDownloadUsers = true,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.MessageContent
        };
        services.AddSingleton(new DiscordSocketClient(discordConfig));
        services.AddSingleton(new CommandService());
        services.AddSingleton<CommandHandler>();
        
        services.AddTransient<IMessages, EnglishMessages>();
        
        // Commands
        services.AddTransient<StatusCommand>();
        services.AddTransient<OpenCommand>();
        services.AddTransient<ToggleAdminCommand>();
        services.AddTransient<SetMaxPriceCommand>();
        services.AddTransient<JoinCommand>();
        services.AddTransient<DrawCommand>();
        services.AddTransient<WhoCommand>();
        services.AddTransient<SentCommand>();
        
        // Permissions
        services.AddTransient<ICanSetMaxPrice, CanSetMaxPrice>();
        services.AddTransient<ICanStartDraw, CanStartDraw>();
        
        return new SecretSantaServices(services);
    }
}