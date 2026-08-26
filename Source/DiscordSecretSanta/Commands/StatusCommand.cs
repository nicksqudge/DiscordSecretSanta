using System.Text;

namespace DiscordSecretSanta.Commands;

public class StatusCommand : AbstractCommand<StatusCommand.Input, StatusCommand.Output>
{
    public sealed record Input : ICommandInput;

    public sealed record Output : ICommandOutput
    {
        public StringBuilder Reply { get; set; } = null!;
    }

    public StatusCommand(IDataStore dataStore, IMessages messages) : base(dataStore, messages)
    {
    }

    protected override async Task<Output> HandleAction(Input input, CancellationToken cancellationToken)
    {
        var status = await DataStore.GetStatus(cancellationToken);
        var result = new StringBuilder();

        switch (status)
        {
            case CampaignStatusId.Ready:
                result.AppendLine(Messages.StatusIsReady());
                break;
            
            case CampaignStatusId.Drawn:
                result.AppendLine(Messages.StatusIsDrawn());
                break;
            
            case CampaignStatusId.Open:
                var memberCount = await DataStore.GetNumberOfMembers(cancellationToken);
                result.AppendLine(Messages.StatusIsOpen(memberCount));
                break;
            
            case CampaignStatusId.NotConfigured:
                result.AppendLine(Messages.StatusIsNotConfigured());
                break;
         
            default:
                result.AppendLine($"I don't know my status... {status} is not supported :(");
                break;
        }

        if (status != CampaignStatusId.NotConfigured)
        {
            var config = await DataStore.GetConfig(cancellationToken);
            result.AppendLine(Messages.StatusMaxPrice(config.MaxPrice));
        }

        return new Output()
        {
            Reply = result
        };
    }
}