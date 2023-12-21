using DiscordSecretSanta.Controllers.Home;

namespace DiscordSecretSanta.Tests.Controllers;

public class HomeControllerTests : ApiTestFixture
{
    [Test]
    public async Task NoConfigSetup()
    {
        // ARRANGE
        using var api = CreateClient();

        // ACT
        var response = await api.GetAsync(GetHomeController.Route);

        // ASSERT
        response.Should()
            .BeOk();
    }
}