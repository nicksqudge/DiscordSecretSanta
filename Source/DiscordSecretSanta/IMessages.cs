namespace DiscordSecretSanta;

public interface IMessages
{
    string StatusIsReady();
    string StatusIsOpen(int memberCount);
    string StatusIsDrawn();
    string StatusIsNotConfigured();
    string OpenNotConfigured();
    string MustHaveMaxPrice();
    string NowOpen();
    string AlreadyOpen();
    string AlreadyDrawn();
    string IsGuidAdmin(string name);
    string IsNoLongerAnAdmin(string name);
    string IsNowAnAdmin(string name);
    string YouDoNotHavePermissionToMakeAdmin();
}