namespace DiscordSecretSanta.Translations.English;

public class EnglishMessages : Messages
{
    public string StatusIsReady() => "I am not open for people to sign up yet. Just waiting on an admin to say \"ready\"";

    public string StatusIsOpen(int memberCount)
    {
        var message = $"I am open for new people to join, just say \"join\" to me. ";
        if (memberCount == 0)
            message += "No one has signed up yet";
        else
            message += $"{memberCount} people have already signed up";
                
        return message;
    }

    public string StatusIsDrawn() => "All names have been drawn and so I am no longer accepting new people to join.";
}