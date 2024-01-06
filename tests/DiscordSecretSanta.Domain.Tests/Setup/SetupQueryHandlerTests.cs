using DiscordSecretSanta.Domain.Config;
using DiscordSecretSanta.Domain.Setup;
using NSubstitute;

namespace DiscordSecretSanta.Domain.Tests.Setup;

public class SetupQueryHandlerTests
{
    private IConfigProvider _configProvider;
    private SetupQueryHandler _target;

    [SetUp]
    public void Setup()
    {
        _configProvider = Substitute.For<IConfigProvider>();

        _target = new SetupQueryHandler(
            _configProvider,
            new ConfigValidator()
        );
    }

    [Test]
    public async Task NoConfig()
    {
        // ARRANGE
        _configProvider
            .TryGetConfig(Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs<AppConfig?>(callInfo => null);
        var query = new SetupQuery();

        // ACT
        var result = await _target.HandleAsync(query, CancellationToken.None);

        // ASSERT
        result.Should()
            .BeSuccess()
            .And.BeEquivalentTo(new SetupResponse
            {
                User = null,
                ConfigOkay = false,
                ConfigDetails = null,
                ActiveCampaign = null
            });
    }

    [Test]
    public async Task ConfigIsInvalid()
    {
        // ARRANGE
        _configProvider.TryGetConfig(Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(new AppConfig());
        var query = new SetupQuery();

        // ACT
        var result = await _target.HandleAsync(query, CancellationToken.None);

        // ASSERT
        result.Should()
            .BeSuccess()
            .And.BeEquivalentTo(new SetupResponse
            {
                User = null,
                ConfigOkay = false,
                ConfigDetails = new List<SetupConfigResponse>
                {
                    new()
                    {
                        Key = ConfigErrors.ConfigKey,
                        Reason = ConfigErrors.InvalidConfig,
                        IsHealthy = false
                    }
                },
                ActiveCampaign = null
            });
    }
}