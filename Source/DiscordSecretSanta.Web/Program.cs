using DiscordSecretSanta;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDiscordSecretSanta(builder.Configuration);
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build(); 

var web = app.RunAsync();
var discordSecretSanta = SecretSantaBot.RunAsync(app.Services);

await Task.WhenAll(web, discordSecretSanta);