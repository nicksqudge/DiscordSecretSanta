using FluentResults;

namespace DiscordSecretSanta.Core;

public static class FluentResultExtensions
{
    public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> input, Action<T> onAction)
    {
        var result = await input;
        if (result.IsSuccess)
            onAction.Invoke(result.Value);

        return result;
    }
}