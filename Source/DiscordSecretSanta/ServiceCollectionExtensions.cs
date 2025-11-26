using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDiscordSecretSanta(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new Configuration();
        configuration.Bind(config);
        services.AddSingleton(config);

        services.AddSingleton(new DiscordSocketConfig());
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton(new CommandService());
        services.AddSingleton<CommandHandler>();
        
        return services;
    }
}