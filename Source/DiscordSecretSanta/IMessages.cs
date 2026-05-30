using DiscordSecretSanta.Commands;

namespace DiscordSecretSanta;

public interface IMessages
{
    string StatusIsReady();
    string StatusIsOpen(int memberCount);
    string StatusIsDrawn();
    string StatusIsNotConfigured();
    string StatusMaxPrice(string maxPrice);
    string OpenNotConfigured();
    string MustHaveMaxPrice();
    string NowOpen();
    string AlreadyOpen();
    string AlreadyDrawn();
    string IsGuidAdmin(string name);
    string IsNoLongerAnAdmin(string name);
    string IsNowAnAdmin(string name);
    string YouDoNotHavePermissionToMakeAdmin();
    string YouDoNotHavePermissionToDraw();
    string YouAreNotAnAdmin();
    string MaxPriceMustHaveAValue();
    string MaxPriceSaved();
    string NotOpenForJoining();
    string YouHaveAlreadyJoined();
    string YouHaveSuccessfullyJoined();
    string NotAValidWishlistUrl();
    string DrawComplete();
    string CouldNotDraw();
    string SecretSantaDrawnDirectMessage(string guildName, string secretSanta, Uri url);
    string CouldNotShowWho();
    string CouldShow();
    string UnexpectedError(string detail);
}