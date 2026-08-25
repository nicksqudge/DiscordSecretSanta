using System.Text;

namespace DiscordSecretSanta.Commands;

public class OpenCommand : AbstractCommand<OpenCommand.Input, OpenCommand.Output>
{
    public OpenCommand(IDataStore dataStore, IMessages messages) : base(dataStore, messages)
    {
        AllowedStatuses = [ CampaignStatusId.NotConfigured, CampaignStatusId.Ready ];
    }
    
    public sealed record Input : ICommandInput;

    public sealed record Output : ICommandOutput
    {
        public StringBuilder Reply { get; set; } = null!;
    }
    
    protected override async Task<Output> HandleAction(Input input, CancellationToken cancellationToken)
    {
        var result = new StringBuilder();
        var status = await DataStore.GetStatus(cancellationToken);
        var config = await DataStore.GetConfig(cancellationToken);

        if (status == CampaignStatusId.NotConfigured)
        {
            var validator = new SecretSantaConfigValidator(Messages);
            var validationResult = await validator.ValidateAsync(config, cancellationToken);

            if (!validationResult.IsValid)
            {
                result.AppendLine(Messages.OpenNotConfigured());
                result.AppendLines(validationResult.Errors.Select(x => x.ErrorMessage));
                return new Output()
                {
                    Reply = result
                };
            }
        }
        
        await DataStore.SetStatus(CampaignStatusId.Open, cancellationToken);
        result.AppendLine(Messages.NowOpen());
        return new Output()
        {
            Reply = result
        };
    }
}