using DiscordSecretSanta;
using DiscordSecretSanta.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDiscordSecretSanta(builder.Configuration)
    .AddJsonDataStore();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build(); 

await SecretSantaBot.Run(app.Services);

// var web = app.RunAsync();
// var discordSecretSanta = SecretSantaBot.Run(app.Services);
//
// await Task.WhenAll(web, discordSecretSanta);