using System.Text;

namespace DiscordSecretSanta.Translations.English;

public class EnglishMessages : IMessages
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

    public string StatusIsDrawn() => "Secret Santa names have been drawn. If you have signed up but haven't been told yet, make sure that I can message you and then run the command \"who\"";

    public string StatusIsNotConfigured()
        => "A secret santa has not been configured by an admin yet. An admin needs to set the max price first, through the command \"max-price\".";

    public string StatusMaxPrice(string maxPrice)
        => "Max Gift Price is " + maxPrice;

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

    public string IsGuidAdmin(string name)
        => $"{name} are already an admin of this discord server and will always be an admin for secret santa";

    public string IsNoLongerAnAdmin(string name)
        => $"{name} no longer an admin for secret santa";

    public string IsNowAnAdmin(string name)
     => $"{name} is now an admin for secret santa";

    public string YouDoNotHavePermissionToMakeAdmin()
        => "Sorry you do not have permission to make that user a secret santa admin";

    public string YouDoNotHavePermissionToDraw()
        => "Sorry you do not have permission to start the drawing of secret santas";

    public string YouAreNotAnAdmin()
        => "Sorry you are not an admin";

    public string MaxPriceMustHaveAValue()
        => "Max price cannot be empty";

    public string MaxPriceSaved()
        => "Max price has been saved";

    public string NotOpenForJoining()
        => "Sorry, Secret Santa hasn't opened yet";

    public string YouHaveAlreadyJoined()
     => "You have already joined";

    public string YouHaveSuccessfullyJoined()
        => "Thank you, you are now signed up for Secret Santa";

    public string NotAValidWishlistUrl()
        => "That is not a valid wishlist url, please try again.";

    public string DrawComplete()
        => "All secret santas have been drawn";

    public string CouldNotDraw()
        => "Secret santa cannot be drawn at this point";

    public string SecretSantaDrawnDirectMessage(string guildName, string secretSanta, Uri url)
    {
        var result = new StringBuilder();
        result.AppendLine($"Hi, I am the Secret Santa bot from: {guildName}.");
        result.AppendLine($"The Secret Santa's have been drawn and yours is: {secretSanta}.");
        result.AppendLine($"Their wishlist is: {url}");
        result.AppendLine(
            "When you have sent their gift then send me the message \"sent\" and I will let them know their gift is on the way");
        result.AppendLine($"Any issues please let the admins of the Secret Santa know on the {guildName} server");

        return result.ToString();
    }

    public string CouldNotShowWho()
        => "Names have not been drawn yet so I cannot say who you have drawn";

    public string CouldShow()
        => "I have sent you a direct message with who you have drawn for secret santa.";

    public string UnexpectedError(string detail)
        => $"I am sorry an unexpected error has occured. Please contact an admin. Code: {detail}";

    public string StatusNotValidForSent()
        => "The campaign is not in the right stage for you to be able to mark this as sent.";
}