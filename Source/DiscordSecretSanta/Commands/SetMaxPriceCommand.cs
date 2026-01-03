using System.Text;
using DiscordSecretSanta.Permissions;
using DiscordSecretSanta.Validators;

namespace DiscordSecretSanta.Commands;

public class SetMaxPriceCommand(
    IDataStore dataStore, 
    IMessages messages, 
    ICanSetMaxPrice canSetMaxPrice)
{
    public async Task<StringBuilder> Handle(InputUser requestingUser, string maxPrice, CancellationToken cancellationToken)
    {
        if (!await canSetMaxPrice.Can(requestingUser, cancellationToken))
            return new StringBuilder(messages.YouAreNotAnAdmin());

        if (new NotEmptyStringValidator().Validate(maxPrice).IsValid == false)
            return new StringBuilder(messages.MaxPriceMustHaveAValue());

        var status = await dataStore.GetStatus(cancellationToken);
        if (status == Status.Drawn)
            return new StringBuilder(messages.AlreadyDrawn());

        Logger.Debug($"Setting max price: {maxPrice}");
        await dataStore.SetMaxPrice(maxPrice, cancellationToken);
        return new StringBuilder(messages.MaxPriceSaved());
    }
}