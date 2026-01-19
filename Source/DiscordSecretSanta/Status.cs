namespace DiscordSecretSanta;

public enum Status
{
    /// <summary>
    /// The secret santa campaign has not been configured yet and cannot be opened
    /// </summary>
    NotConfigured,
    /// <summary>
    /// The secret santa campaign is configured and ready to be opened
    /// </summary>
    Ready,
    /// <summary>
    /// The secret santa campaign is open and people can join
    /// </summary>
    Open,
    /// <summary>
    /// The secret santa campaign has been drawn and so no one can join
    /// </summary>
    Drawn
}