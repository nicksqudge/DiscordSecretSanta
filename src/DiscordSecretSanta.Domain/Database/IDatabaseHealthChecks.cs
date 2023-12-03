namespace DiscordSecretSanta.Domain.Database;

public interface IDatabaseHealthChecks
{
    /// <summary>
    ///     Define this method to confirm that the database connection details you have provided are valid
    /// </summary>
    /// <returns>True if valid, false if not</returns>
    Task<bool> CanConnectToDatabase();

    /// <summary>
    ///     Define this method to confirm that the database has been initialised and is ready for use with a campaign.
    ///     If not then it will be setup as part of the SetupCampaign method
    /// </summary>
    /// <returns></returns>
    Task<bool> HasDatabaseBeenSetup();
}