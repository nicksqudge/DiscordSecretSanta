using System.Text;

namespace DiscordSecretSanta.Commands;

public class SentCommand(IDataStore dataStore, IMessages messages)
{
    public sealed record DirectMessage(DiscordUserId Receiver);
    
    public async Task<(StringBuilder Response, DirectMessage? ToSend)> Handle(DiscordUserId requestingUserId, CancellationToken cancellationToken)
    {
        var status = await dataStore.GetStatus(cancellationToken);
        if (status != Status.Drawn)
            return ReturnFail(messages.StatusNotValidForSent());
        
        var requester = await dataStore.GetMember(requestingUserId, cancellationToken);
        if (requester is null)
            return UnexpectedError($"UNABLE TO FETCH REQUESTING USER ID: {requestingUserId}");

        if (requester.SecretSantaId is null)
            return UnexpectedError($"REQUESTING USER ID: {requestingUserId} DOES NOT HAVE AN ASSIGNED SECRET SANTA");

        if (requester.SecretSantaStatus != SecretSantaStatus.Pending && requester.SecretSantaStatus is not null)
            return ReturnFail(messages.AlreadySent());
        
        await dataStore.SetSecretSantaStatus(requester.UserId, SecretSantaStatus.Sent, cancellationToken);
        var directMessage = new DirectMessage(requester.SecretSantaId);
        return (new StringBuilder(messages.MarkedAsSent()), directMessage);
    }
    
    private (StringBuilder Response, DirectMessage? ToSend) ReturnFail(string message)
        => (new StringBuilder(message), null);

    private (StringBuilder Response, DirectMessage? ToSend) UnexpectedError(string error)
        => ReturnFail(messages.UnexpectedError(nameof(SentCommand), error));
}