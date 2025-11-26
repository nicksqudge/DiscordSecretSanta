using Discord;

namespace DiscordSecretSanta;

public class Logger
{
    public static Task Log(LogMessage msg)
    {
        Console.WriteLine("SECRETSANTA: " + msg.ToString());
        return Task.CompletedTask;
    }
}