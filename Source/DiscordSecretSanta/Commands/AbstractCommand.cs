using System.Text;

namespace DiscordSecretSanta.Commands;

public abstract class AbstractCommand<TInput, TResponse> 
    where TInput : class, ICommandInput
    where TResponse : class, ICommandResponse, new ()
{
    protected IDataStore DataStore;
    protected IMessages Messages;
    protected CampaignStatusId[] AllowedStatuses = [];

    public AbstractCommand(IDataStore dataStore, IMessages messages)
    {
        DataStore = dataStore;
        Messages = messages;
    }
    
    public async Task<TResponse> Handle(TInput input, CancellationToken cancellationToken)
    {
        var status = await DataStore.GetStatus(cancellationToken);
        if (!AllowedStatuses.Contains(status))
        {
            var response = new TResponse()
            {
                Output = Messages.StatusNotSupported(status)
            };
            return response as TResponse;
        }

        try
        {
            return await HandleAction(input, cancellationToken);
        }
        catch (Exception e)
        {
            return new TResponse()
            {
                Output = new StringBuilder(e.Message).AppendLine("Command: " + this.GetType().Name)
            };
        }
    }
    
    protected abstract Task<TResponse> HandleAction(TInput input, CancellationToken cancellationToken);
}

public interface ICommandInput
{
    
}

public interface ICommandResponse
{
    StringBuilder Output { get; set; }
}
