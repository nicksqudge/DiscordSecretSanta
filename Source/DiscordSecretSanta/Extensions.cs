using System.Text;

namespace DiscordSecretSanta;

public static class Extensions
{
    public static StringBuilder AppendLines(this StringBuilder builder, IEnumerable<string> lines)
    {
        lines.ToList().ForEach(line => builder.AppendLine(line));
        return builder;
    }
}