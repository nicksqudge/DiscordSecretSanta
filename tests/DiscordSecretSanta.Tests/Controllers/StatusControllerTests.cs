using DiscordSecretSanta.Controllers.Status;
using DiscordSecretSanta.Domain.Status;

namespace DiscordSecretSanta.Tests.Controllers;

public class StatusControllerTests : ApiTestFixture
{
    [Test]
    public async Task NothingSetup()
    {
        // ARRANGE
        using var api = CreateClient();

        // ACT
        var response = await api.GetAsync(StatusController.Route);

        // ASSERT
        await response.Should()
            .BeOk()
            .And.MatchesResponse(new StatusResponse
            {
                Installed = false,
                CampaignSetup = false
            });
    }
}