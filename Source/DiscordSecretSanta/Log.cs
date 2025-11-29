using Discord;

namespace DiscordSecretSanta;

public class Logger
{
    public static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    public static void Debug(string message)
    {
        #if DEBUG
            Console.WriteLine("DEBUG: " + message);
        #endif
    }
}