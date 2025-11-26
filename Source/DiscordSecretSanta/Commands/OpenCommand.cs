namespace DiscordSecretSanta.Commands;

public class OpenCommand
{
    private readonly DataStore _dataStore;
    private readonly Messages _messages;

    public OpenCommand(DataStore dataStore, Messages messages)
    {
        _dataStore = dataStore;
        _messages = messages;
    }

    public async Task<string[]> Handle(CancellationToken token)
    {
        var status = await _dataStore.GetStatus(token);
        var config = await _dataStore.GetConfig(token);

        if (status == Status.NotConfigured)
        {
            var validator = new SecretSantaConfigValidator(_messages);
            var validationResult = await validator.ValidateAsync(config, token);

            if (!validationResult.IsValid)
            {
                var result = new List<string>();
                result.Add(_messages.OpenNotConfigured());
                result.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return result.ToArray();
            }
        }
        
        await _dataStore.SetStatus(Status.Open, token);
        return [_messages.NowOpen()];
    }
}