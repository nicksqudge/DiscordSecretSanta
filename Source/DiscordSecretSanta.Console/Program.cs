// See https://aka.ms/new-console-template for more information

using DiscordSecretSanta;
using DiscordSecretSanta.DataStore.Json;
using DiscordSecretSanta.WishlistUrlValidators;
using Microsoft.Extensions.DependencyInjection;

var token = Environment.GetEnvironmentVariable("DiscordSecretSanta_Token");
if (string.IsNullOrEmpty(token))
{
    Console.WriteLine("Please set DiscordSecretSanta_Token in Environment Variables");
    return;
}

var services = new ServiceCollection()
    .AddDiscordSecretSanta(token)
    .AddJsonDataStore()
    .AddWishlistValidator<AmazonWishlistValidator>()
    .Services.BuildServiceProvider();
    
await SecretSantaBot.Run(services);