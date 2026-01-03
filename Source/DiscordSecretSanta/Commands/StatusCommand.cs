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
            case Status.Ready:
                result.AppendLine(_messages.StatusIsReady());
                break;
            
            case Status.Drawn:
                result.AppendLine(_messages.StatusIsDrawn());
                break;
            
            case Status.Open:
                var memberCount = await _dataStore.GetNumberOfMembers(cancellationToken);
                result.AppendLine(_messages.StatusIsOpen(memberCount));
                break;
            
            case Status.NotConfigured:
                result.AppendLine(_messages.StatusIsNotConfigured());
                break;
         
            default:
                result.AppendLine($"I don't know my status... {status} is not supported :(");
                break;
        }

        if (status != Status.NotConfigured)
        {
            var config = await _dataStore.GetConfig(cancellationToken);
            result.AppendLine(_messages.StatusMaxPrice(config.MaxPrice));
        }

        return result;
    }
}