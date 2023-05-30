using Blazorise;
using DiscordSecretSanta.Blazor.Shared;
using DiscordSecretSanta.Core;
using Microsoft.AspNetCore.Components;

namespace DiscordSecretSanta.Blazor;

public static class ModalServiceExtensions
{
    public static Task<ModalInstance> ShowSelectStatusModal(this IModalService modalService, SecretSantaStatus current, IEnumerable<SecretSantaStatus> available, Func<SecretSantaStatus, Task> onSelect)
    {
        return modalService.Show<SelectStatusModal>(x =>
            {
                x.Add(z => z.Current, current);
                x.Add(z => z.AvailableStatuses, available);
                x.Add(z => z.OnSelect, onSelect); 
            }, 
            new ModalInstanceOptions()
            {
                UseModalStructure = false,
                Centered = true
            }
        );
    }
}