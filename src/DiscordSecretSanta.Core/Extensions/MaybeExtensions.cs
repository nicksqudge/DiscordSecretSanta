using CSharpFunctionalExtensions;

namespace DiscordSecretSanta.Core.Extensions;

public static class MaybeExtensions
{
    public static async Task OnHasValue<T>(this Task<Maybe<T>> input, Action<T> action)
    {
        var result = await input;
        if (result.HasValue)
            action.Invoke(result.Value);
    }
}