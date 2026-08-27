using System.Text;

namespace DiscordSecretSanta.Commands;

public class JoinCommand : AbstractCommand<JoinCommand.Input, JoinCommand.Output>
{
    private readonly IEnumerable<IWishlistUrlValidator> _validators;

    public sealed record Input(DiscordUserId UserId, string WishlistUrl) : ICommandInput;

    public sealed record Output: ICommandOutput
    {
        public StringBuilder Reply { get; set; } = null!;
    }

    public JoinCommand(IDataStore dataStore, IMessages messages, IEnumerable<IWishlistUrlValidator> validators) : base(dataStore, messages)
    {
        _validators = validators;
        AllowedStatuses = [CampaignStatusId.Open];
    }

    protected override async Task<Output> HandleAction(Input input, CancellationToken cancellationToken)
    {
        var validWishlistUrl = await IsValidWishlistUrl(input.WishlistUrl, cancellationToken);
        if (validWishlistUrl is null)
            return ReturnMessage(Messages.NotAValidWishlistUrl());

        var memberExists = await DoesMemberAlreadyExist(input.UserId, cancellationToken);
        if (memberExists)
            return ReturnMessage(Messages.YouHaveAlreadyJoined());
        
        await DataStore.AddMember(input.UserId, validWishlistUrl, cancellationToken);
        Logger.Debug($"User joined {input.UserId}");

        return ReturnMessage(Messages.YouHaveSuccessfullyJoined());
    }

    private Output ReturnMessage(string message)
        => new()
        {
            Reply = new StringBuilder(message)
        };

    private async Task<bool> DoesMemberAlreadyExist(DiscordUserId userId, CancellationToken cancellationToken)
    {
        var member = await DataStore.GetMember(userId, cancellationToken);
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