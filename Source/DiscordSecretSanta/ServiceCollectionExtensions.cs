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
    public static SecretSantaServices AddDiscordSecretSanta(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new Configuration();
        configuration.Bind(config);
        services.AddSingleton(config);

        services.AddSingleton(new DiscordSocketConfig());
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton(new CommandService());
        services.AddSingleton<CommandHandler>();
        
        services.AddTransient<Messages, EnglishMessages>();
        services.AddTransient<StatusCommand>();
        
        return new SecretSantaServices(services);
    }
}