using System.Runtime.CompilerServices;
using DiscordSecretSanta.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DiscordSecretSanta.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IMongoDatabase database)
    {
        return services
            .AddSingleton(database)
            .AddTransient<IUserRepository, UserRepository>();
    }
}