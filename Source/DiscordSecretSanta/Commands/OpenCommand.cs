namespace DiscordSecretSanta.Commands;

public class OpenCommand
{
    private readonly IDataStore _dataStore;
    private readonly Messages _messages;

    public OpenCommand(IDataStore dataStore, Messages messages)
    {
        _dataStore = dataStore;
        _messages = messages;
    }

    public async Task<string[]> Handle(CancellationToken token)
    {
        var status = await _dataStore.GetStatus(token);
        var config = await _dataStore.GetConfig(token);

        switch (status)
        {
            case Status.NotConfigured:
                var validator = new SecretSantaConfigValidator(_messages);
                var validationResult = await validator.ValidateAsync(config, token);

                if (!validationResult.IsValid)
                {
                    var result = new List<string>();
                    result.Add(_messages.OpenNotConfigured());
                    result.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                    return result.ToArray();
                }

                break;
            
            case Status.Drawn:
                return [_messages.AlreadyDrawn()];
            
            case Status.Open:
                return [_messages.AlreadyOpen()];
        }
        
        await _dataStore.SetStatus(Status.Open, token);
        return [_messages.NowOpen()];
    }
}