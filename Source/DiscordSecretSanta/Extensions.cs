using System.Text;

namespace DiscordSecretSanta;

public static class Extensions
{
    public static StringBuilder AppendLines(this StringBuilder builder, IEnumerable<string> lines)
    {
        lines.ToList().ForEach(line => builder.AppendLine(line));
        return builder;
    }
    
    public static void Shuffle<T> (this Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}