using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Json;

public static class Extensions
{
    public static SecretSantaServices AddJsonDataStore(this SecretSantaServices services)
    {
        services.Services.AddTransient<DataStore, JsonDataStore>();
        return services;
    }
}