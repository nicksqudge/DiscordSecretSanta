using DiscordSecretSanta.Domain.Config;
using DotnetCQRS;
using DotnetCQRS.Queries;
using FluentValidation;

namespace DiscordSecretSanta.Domain.Setup;

public class SetupQueryHandler : IQueryHandler<SetupQuery, SetupResponse>
{
    private readonly IConfigProvider _configProvider;
    private readonly IValidator<AppConfig> _validator;

    public SetupQueryHandler(IConfigProvider configProvider, IValidator<AppConfig> validator)
    {
        _configProvider = configProvider;
        _validator = validator;
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

        var validationResult = await _validator.ValidateAsync(config, cancellationToken);
        if (validationResult.IsValid == false)
            return Result.Success(new SetupResponse
            {
                User = null,
                ActiveCampaign = null,
                ConfigDetails = validationResult.Errors
                    .Select(x => new SetupConfigResponse
                    {
                        Key = ConfigErrors.ConfigKey,
                        IsHealthy = false,
                        Reason = x.ErrorCode
                    })
                    .ToList(),
                ConfigOkay = false
            });

        throw new NotImplementedException();
    }
}