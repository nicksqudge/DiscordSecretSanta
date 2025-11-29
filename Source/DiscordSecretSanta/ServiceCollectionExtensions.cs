using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Translations.English;
using Microsoft.Extensions.Configuration;
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
        services.AddTransient<StatusCommand>();
        services.AddTransient<OpenCommand>();
        services.AddTransient<ToggleAdminCommand>();
        
        return new SecretSantaServices(services);
    }
}