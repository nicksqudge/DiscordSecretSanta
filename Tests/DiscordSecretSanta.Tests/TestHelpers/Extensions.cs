using System.Text;

namespace DiscordSecretSanta.Tests.TestHelpers;

public static class Extensions
{
    public static StringBuilder ToStringBuilder(this string[] input)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLines(input);
        return stringBuilder;
    }
}