using System.Text;

namespace DiscordSecretSanta.Commands;

public class ArrivedCommand(IDataStore dataStore, IMessages messages)
{
    public sealed record DirectMessage(DiscordUserId Sender);

    public async Task<(StringBuilder Response, DirectMessage? ToSend)> Handle(DiscordUserId requestingUserId,
        CancellationToken cancellationToken)
    {
        var status = await dataStore.GetStatus(cancellationToken);
        if (status < Status.Drawn)
            return ReturnFail(messages.StatusNotValidForArrived());
        
        var secretSanta = await dataStore.GetMembersSecretSanta(requestingUserId, cancellationToken);
        if (secretSanta is null)
            return UnexpectedError($"UNABLE TO FETCH SECRET SANTA FOR USER ID: {requestingUserId}");

        if (secretSanta.SecretSantaId is null || secretSanta.SecretSantaId != requestingUserId)
            return UnexpectedError(
                $"THE FETCHED SECRET SANTA OF {requestingUserId} IS UNEXPECTEDLY {secretSanta.SecretSantaId}");
        
        if (secretSanta.SecretSantaStatus == SecretSantaStatus.Arrived)
            return ReturnFail(messages.AlreadyArrived());
        
        await dataStore.SetSecretSantaStatus(secretSanta.UserId, SecretSantaStatus.Arrived, cancellationToken);
        var directMessage = new DirectMessage(secretSanta.UserId);
        return (new StringBuilder(messages.MarkedAsArrived()), directMessage);
    }
    
    private (StringBuilder Response, DirectMessage? ToSend) ReturnFail(string message)
        => (new StringBuilder(message), null);
    
    private (StringBuilder Response, DirectMessage? ToSend) UnexpectedError(string error)
        => ReturnFail(messages.UnexpectedError(nameof(SentCommand), error));
}