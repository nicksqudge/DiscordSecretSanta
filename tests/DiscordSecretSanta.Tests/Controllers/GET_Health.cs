using System.Net;
using DiscordSecretSanta.Controllers;
using DiscordSecretSanta.Domain.Database;
using NSubstitute;

namespace DiscordSecretSanta.Tests.Controllers;

public class GetHealth : ApiTestFixture
{
    [Test]
    public async Task AllWorking()
    {
        // ARRANGE
        var dbHealthCheck = Substitute.For<IDatabaseHealthChecks>();
        dbHealthCheck.CanConnectToDatabase()
            .ReturnsForAnyArgs(true);
        using var api = CreateClient();

        // ACT
        var response = await api.GetAsync(RootController.HealthRoute);

        // ASSERT
        await response.Should()
            .BeOk()
            .And.HaveContent("Healthy");
    }

    [Test]
    public async Task DatabaseNotSetup()
    {
        // ARRANGE
        var dbHealthCheck = Substitute.For<IDatabaseHealthChecks>();
        dbHealthCheck.CanConnectToDatabase()
            .ReturnsForAnyArgs(false);

        using var api = CreateClient(services => { services.AddDatabaseHealthCheck(dbHealthCheck); });

        // ACT
        var response = await api.GetAsync(RootController.HealthRoute);

        // ASSERT
        await response.Should()
            .HaveStatusCode(HttpStatusCode.ServiceUnavailable)
            .And.HaveContent("Unhealthy");
    }
}