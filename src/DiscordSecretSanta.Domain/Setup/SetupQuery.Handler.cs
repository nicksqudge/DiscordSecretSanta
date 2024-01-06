using DotnetCQRS;
using DotnetCQRS.Queries;

namespace DiscordSecretSanta.Domain.Setup;

public class SetupQueryHandler : IQueryHandler<SetupQuery, SetupResponse>
{
    private readonly IConfigProvider _configProvider;

    public SetupQueryHandler(IConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }

    public async Task<Result<SetupResponse>> HandleAsync(SetupQuery query, CancellationToken cancellationToken)
    {
        var config = await _configProvider.TryGetConfig(cancellationToken);
        if (config is null)
            return Result.Success(new SetupResponse
            {
                User = null,
                ConfigOkay = false,
                ConfigDetails = null,
                ActiveCampaign = null
            });

        throw new NotImplementedException();
    }
}