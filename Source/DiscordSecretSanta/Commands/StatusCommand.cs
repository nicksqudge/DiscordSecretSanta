using System.Text;

namespace DiscordSecretSanta.Commands;

public class StatusCommand
{
    private readonly IDataStore _dataStore;
    private readonly IMessages _messages;

    public StatusCommand(IDataStore dataStore, IMessages messages)
    {
        _dataStore = dataStore;
        _messages = messages;
    }

    public async Task<StringBuilder> Handle(CancellationToken cancellationToken)
    {
        var status = await _dataStore.GetStatus(cancellationToken);
        var result = new StringBuilder();

        switch (status)
        {
            case CampaignStatusId.Ready:
                result.AppendLine(_messages.StatusIsReady());
                break;
            
            case CampaignStatusId.Drawn:
                result.AppendLine(_messages.StatusIsDrawn());
                break;
            
            case CampaignStatusId.Open:
                var memberCount = await _dataStore.GetNumberOfMembers(cancellationToken);
                result.AppendLine(_messages.StatusIsOpen(memberCount));
                break;
            
            case CampaignStatusId.NotConfigured:
                result.AppendLine(_messages.StatusIsNotConfigured());
                break;
         
            default:
                result.AppendLine($"I don't know my status... {status} is not supported :(");
                break;
        }

        if (status != CampaignStatusId.NotConfigured)
        {
            var config = await _dataStore.GetConfig(cancellationToken);
            result.AppendLine(_messages.StatusMaxPrice(config.MaxPrice));
        }

        return result;
    }
}