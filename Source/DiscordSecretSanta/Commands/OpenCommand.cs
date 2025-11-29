using System.Text;

namespace DiscordSecretSanta.Commands;

public class OpenCommand
{
    private readonly IDataStore _dataStore;
    private readonly IMessages _messages;

    public OpenCommand(IDataStore dataStore, IMessages messages)
    {
        _dataStore = dataStore;
        _messages = messages;
    }

    public async Task<StringBuilder> Handle(CancellationToken token)
    {
        var result = new StringBuilder();
        var status = await _dataStore.GetStatus(token);
        var config = await _dataStore.GetConfig(token);

        switch (status)
        {
            case Status.NotConfigured:
                var validator = new SecretSantaConfigValidator(_messages);
                var validationResult = await validator.ValidateAsync(config, token);

                if (!validationResult.IsValid)
                {
                    result.AppendLine(_messages.OpenNotConfigured());
                    result.AppendLines(validationResult.Errors.Select(x => x.ErrorMessage));
                    return result;
                }

                break;
            
            case Status.Drawn:
                return result.AppendLine(_messages.AlreadyDrawn());
            
            case Status.Open:
                return result.AppendLine(_messages.AlreadyOpen());
        }
        
        await _dataStore.SetStatus(Status.Open, token);
        return result.AppendLine(_messages.NowOpen());
    }
}