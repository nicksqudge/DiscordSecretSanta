namespace DiscordSecretSanta.Tests.TestHelpers;

public class TestFactory
{
    public static InputUser InputUser(bool isAdmin = false)
    {
        var id = LongRandom();
        return new InputUser(new DiscordUserId(id), $"Test {id}", isAdmin);
    }

    private static ulong LongRandom()
    {
        var rand = new Random();
        return (ulong)rand.Next(0, int.MaxValue);
    }
}