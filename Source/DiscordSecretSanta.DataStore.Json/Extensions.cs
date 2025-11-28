using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.DataStore.Json;

public static class Extensions
{
    public static SecretSantaServices AddJsonDataStore(this SecretSantaServices services)
    {
        services.Services.AddTransient<IDataStore, JsonDataStore>();
        return services;
    }
}