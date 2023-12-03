namespace DiscordSecretSanta.Domain.Database;

public interface IDatabaseHealthChecks
{
    /// <summary>
    ///     Define this method to confirm that the database connection details you have provided are valid
    /// </summary>
    /// <returns>True if valid, false if not</returns>
    Task<bool> CanConnectToDatabase();
}