using System.Text;

namespace DiscordSecretSanta.Commands;

public abstract class AbstractCommand<TInput, TOutput> 
    where TInput : class, ICommandInput
    where TOutput : class, ICommandOutput, new ()
{
    protected IDataStore DataStore;
    protected IMessages Messages;
    protected CampaignStatusId[] AllowedStatuses = [];

    public AbstractCommand(IDataStore dataStore, IMessages messages)
    {
        DataStore = dataStore;
        Messages = messages;
    }
    
    public async Task<TOutput> Handle(TInput input, CancellationToken cancellationToken)
    {
        var status = await DataStore.GetStatus(cancellationToken);
        if (!AllowedStatuses.Contains(status))
        {
            var response = new TOutput()
            {
                Reply = Messages.StatusNotSupported(status)
            };
            return response;
        }

        try
        {
            return await HandleAction(input, cancellationToken);
        }
        catch (Exception e)
        {
            return new TOutput()
            {
                Reply = new StringBuilder(e.Message).AppendLine("Command: " + this.GetType().Name)
            };
        }
    }
    
    protected abstract Task<TOutput> HandleAction(TInput input, CancellationToken cancellationToken);
}

public interface ICommandInput
{
    
}

public interface ICommandOutput
{
    StringBuilder Reply { get; set; }
}
