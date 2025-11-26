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

    public string StatusIsNotConfigured()
        => "A secret santa has not been configured by an admin yet. They just need to say \"setup\" and I will start the setup process with them";

    public string OpenNotConfigured()
        => "Sorry I cannot be opened, I have not been configured. Please make sure the following is configured: ";

    public string MustHaveMaxPrice()
        => "You must have a max price defined. An admin must call \"max-price\" with the max price for it to be set.";

    public string NowOpen()
        => "Your Secret Santa is now open for people to join";

    public string AlreadyOpen()
     => "I am already open for people to join";

    public string AlreadyDrawn()
        => "The secret santa names have already been drawn, I cannot be opened";
}