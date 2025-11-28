// See https://aka.ms/new-console-template for more information

using DiscordSecretSanta;
using DiscordSecretSanta.DataStore.Json;
using Microsoft.Extensions.DependencyInjection;

var token = Environment.GetEnvironmentVariable("DiscordSecretSanta_Token");
var services = new ServiceCollection()
    .AddDiscordSecretSanta(token)
    .AddJsonDataStore()
    .Services.BuildServiceProvider();
    
await SecretSantaBot.Run(services);