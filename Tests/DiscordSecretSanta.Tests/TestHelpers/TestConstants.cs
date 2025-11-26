namespace DiscordSecretSanta.Tests.TestHelpers;

public class TestConstants
{
    public static SecretSantaConfig ValidConfig() => new()
    {
        MaxPrice = "£10"
    };
}