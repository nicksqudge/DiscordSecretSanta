using System.Text;
using DiscordSecretSanta.Commands;

namespace DiscordSecretSanta;

public interface IMessages
{
    StringBuilder StatusNotSupported(CampaignStatusId status);
    
    string StatusIsReady();
    string StatusIsOpen(int memberCount);
    string StatusIsDrawn();
    string StatusIsNotConfigured();
    string StatusMaxPrice(string maxPrice);
    string StatusIsClosed();
    string OpenNotConfigured();
    string MustHaveMaxPrice();
    string NowOpen();
    string IsGuidAdmin(string name);
    string IsNoLongerAnAdmin(string name);
    string IsNowAnAdmin(string name);
    string YouDoNotHavePermissionToMakeAdmin();
    string YouDoNotHavePermissionToDraw();
    string YouAreNotAnAdmin();
    string MaxPriceMustHaveAValue();
    string MaxPriceSaved();
    string YouHaveAlreadyJoined();
    string YouHaveSuccessfullyJoined();
    string NotAValidWishlistUrl();
    string DrawComplete();
    string CouldNotDraw();
    string SecretSantaDrawnDirectMessage(string guildName, string secretSanta, Uri url);
    string CouldNotShowWho();
    string CouldShow();
    string StatusNotValidForSent();
    string AlreadySent();
    string MarkedAsSent();
    string YourGiftIsOnTheWay();
    string AlreadyArrived();
    string MarkedAsArrived();
    string YourGiftHasArrived();
    string CampaignClosed();
    string CampaignNotClosed();
}