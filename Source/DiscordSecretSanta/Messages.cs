namespace DiscordSecretSanta;

public interface Messages
{
    string StatusIsReady();
    string StatusIsOpen(int memberCount);
    string StatusIsDrawn();
}