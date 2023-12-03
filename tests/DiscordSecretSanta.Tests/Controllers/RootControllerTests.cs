using DiscordSecretSanta.Controllers;

namespace DiscordSecretSanta.Tests.Controllers;

public class RootControllerTests : ApiTestFixture
{
    [Test]
    public async Task GET_health()
    {
        using var api = CreateClient();

        var response = await api.GetAsync(RootController.HealthRoute);

        await response.Should()
            .BeOk()
            .And.HaveContent("Healthy");
    }
}