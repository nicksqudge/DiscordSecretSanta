namespace DiscordSecretSanta;

public interface Messages
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
}