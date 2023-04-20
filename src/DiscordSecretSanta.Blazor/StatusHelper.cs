using Blazorise;
using DiscordSecretSanta.Blazor.Shared;
using DiscordSecretSanta.Core;
using Microsoft.Extensions.Localization;

namespace DiscordSecretSanta.Blazor;

internal static class StatusHelper
{
    public static Color GetColourForStatus(SecretSantaStatus? status)
        => status switch
        {
            SecretSantaStatus.Unassigned => Color.Danger,
            SecretSantaStatus.Arrived => Color.Success,
            SecretSantaStatus.Assigned => Color.Warning,
            SecretSantaStatus.Posted => Color.Info,
            _ => Color.Danger
        };
    
    public static string GetShortText(this IStringLocalizer<SharedResources> translate, SecretSantaStatus? status)
        => status switch
        {
            SecretSantaStatus.Unassigned => translate["No Secret Santa yet"],
            SecretSantaStatus.Arrived => translate["Gift has arrived"],
            SecretSantaStatus.Assigned => translate["Secret Santa assigned"],
            SecretSantaStatus.Posted => translate["Secret Santa gift en-route"],
            _ => translate["No status set"]
        };
}