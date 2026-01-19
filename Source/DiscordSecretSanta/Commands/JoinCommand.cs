using System.Text;

namespace DiscordSecretSanta.Commands;

public class JoinCommand
{
    private readonly IDataStore _dataStore;
    private readonly IMessages _messages;
    private readonly IEnumerable<IWishlistUrlValidator> _validators;

    public JoinCommand(IDataStore dataStore, IMessages messages, IEnumerable<IWishlistUrlValidator> validators)
    {
        _dataStore = dataStore;
        _messages = messages;
        _validators = validators;
    }

    public async Task<StringBuilder> Handle(DiscordUserId userId, string wishlistUrl, CancellationToken cancellationToken)
    {
        var status = await _dataStore.GetStatus(cancellationToken);
        if (status != Status.Open)
            return new StringBuilder(_messages.NotOpenForJoining());

        var validWishlistUrl = await IsValidWishlistUrl(wishlistUrl, cancellationToken);
        if (validWishlistUrl is null)
            return new StringBuilder(_messages.NotAValidWishlistUrl());

        var memberExists = await DoesMemberAlreadyExist(userId, cancellationToken);
        if (memberExists)
            return new StringBuilder(_messages.YouHaveAlreadyJoined());
        
        await _dataStore.AddMember(userId, validWishlistUrl, cancellationToken);
        Logger.Debug($"User joined {userId}");

        return new StringBuilder(_messages.YouHaveSuccessfullyJoined());
    }

    private async Task<bool> DoesMemberAlreadyExist(DiscordUserId userId, CancellationToken cancellationToken)
    {
        var member = await _dataStore.GetMember(userId, cancellationToken);
        Logger.Debug(member is null ? "Member doesnt already exist" : "Member does already exist");
        return member != null;
    }

    private async Task<Uri?> IsValidWishlistUrl(string wishlistUrl, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            Logger.Warning("There are no wishlist url validators configured");
            return null;
        }
        
        foreach (var validator in _validators)
        {
            var validUrl = await validator.IsValid(wishlistUrl, cancellationToken);
            if (validUrl is not null)
            {
                Logger.Debug($"Valid url: {wishlistUrl}");
                return validUrl;
            }
        }

        Logger.Debug($"Could not find url to be valid: {wishlistUrl}");
        return null;
    }
}