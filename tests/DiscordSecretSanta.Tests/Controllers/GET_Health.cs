using System.Net;
using DiscordSecretSanta.Controllers;
using DiscordSecretSanta.Domain.Database;

namespace DiscordSecretSanta.Tests.Controllers;

public class GetHealth : ApiTestFixture
{
    [Test]
    public async Task AllWorking()
    {
        // ARRANGE
        using var api = CreateClient(services => services.AddDatabaseHealthCheck<AllGoodHealthCheck>());

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
        using var api = CreateClient(services => services.AddDatabaseHealthCheck<NoDatabaseConnectionHealthCheck>());

        // ACT
        var response = await api.GetAsync(RootController.HealthRoute);

        // ASSERT
        await response.Should()
            .HaveStatusCode(HttpStatusCode.ServiceUnavailable)
            .And.HaveContent("Unhealthy");
    }

    private class NoDatabaseConnectionHealthCheck : IDatabaseHealthChecks
    {
        public Task<bool> CanConnectToDatabase()
        {
            return Task.FromResult(false);
        }

        public Task<bool> HasDatabaseBeenSetup()
        {
            return Task.FromResult(true);
        }
    }

    private class AllGoodHealthCheck : IDatabaseHealthChecks
    {
        public Task<bool> CanConnectToDatabase()
        {
            return Task.FromResult(true);
        }

        public Task<bool> HasDatabaseBeenSetup()
        {
            return Task.FromResult(true);
        }
    }
}