using System.Text;
using DiscordSecretSanta.Permissions;
using DiscordSecretSanta.Validators;

namespace DiscordSecretSanta.Commands;

public class SetMaxPriceCommand : AbstractCommand<SetMaxPriceCommand.Input, SetMaxPriceCommand.Output>
{
    public sealed record Input(InputUser RequestingUser, string MaxPrice) : ICommandInput;

    public sealed record Output : ICommandOutput
    {
        public StringBuilder Reply { get; set; } = null!;
    }

    private readonly ICanSetMaxPrice _canSetMaxPrice;
    
    public SetMaxPriceCommand(IDataStore dataStore, IMessages messages, ICanSetMaxPrice canSetMaxPrice) : base(dataStore, messages)
    {
        _canSetMaxPrice = canSetMaxPrice;
        AllowedStatuses = [CampaignStatusId.Ready, CampaignStatusId.NotConfigured];
    }

    protected override async Task<Output> HandleAction(Input input, CancellationToken cancellationToken)
    {
        if (!await _canSetMaxPrice.Can(input.RequestingUser, cancellationToken))
            return ReturnMessage(Messages.YouAreNotAnAdmin());

        if (new NotEmptyStringValidator().Validate(input.MaxPrice).IsValid == false)
            return ReturnMessage(Messages.MaxPriceMustHaveAValue());
        
        Logger.Debug($"Setting max price: {input.MaxPrice}");
        await DataStore.SetMaxPrice(input.MaxPrice, cancellationToken);
        return ReturnMessage(Messages.MaxPriceSaved());
    }

    private Output ReturnMessage(string message)
        => new()
        {
            Reply = new StringBuilder(message)
        };
}