namespace DiscordSecretSanta.Commands;

public class StatusCommand
{
    private readonly DataStore _dataStore;
    private readonly Messages _messages;

    public StatusCommand(DataStore dataStore, Messages messages)
    {
        _dataStore = dataStore;
        _messages = messages;
    }

    public async Task<string> Handle(CancellationToken cancellationToken)
    {
        var status = await _dataStore.GetStatus(cancellationToken);

        switch (status)
        {
            case Status.Ready:
                return _messages.StatusIsReady();
            
            case Status.Drawn:
                return _messages.StatusIsDrawn();
            
            case Status.Open:
                var memberCount = await _dataStore.GetNumberOfMembers(cancellationToken);
                return _messages.StatusIsOpen(memberCount);
         
            default:
                return $"I don't know my status... {status} is not supported :(";
        }
    }
}