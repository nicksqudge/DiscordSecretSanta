namespace DiscordSecretSanta.Tests;

public static class Extensions
{
    public static ResponseMessageAssertions Should(this HttpResponseMessage message)
    {
        return new ResponseMessageAssertions(message);
    }
}