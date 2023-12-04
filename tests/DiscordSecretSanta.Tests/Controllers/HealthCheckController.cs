using System.Net;
using DiscordSecretSanta.Configure.HealthChecks;
using DiscordSecretSanta.Domain.HealthCheck;

namespace DiscordSecretSanta.Tests.Controllers;

public class HealthCheckController : ApiTestFixture
{
    [Test]
    public async Task AllWorking()
    {
        // ARRANGE
        using var api = CreateClient(services => services.AddDatabaseHealthCheck<AllGoodHealthCheck>());

        // ACT
        var response = await api.GetAsync(DiscordSecretSanta.Controllers.HealthCheckController.HealthRoute);

        // ASSERT
        await response.Should()
            .BeOk()
            .And.Match<HealthResult>(r => r.Status == "Healthy");
    }

    [Test]
    public async Task DatabaseNotSetup()
    {
        // ARRANGE
        using var api = CreateClient(services => services.AddDatabaseHealthCheck<NoDatabaseConnectionHealthCheck>());

        // ACT
        var response = await api.GetAsync(DiscordSecretSanta.Controllers.HealthCheckController.HealthRoute);

        // ASSERT
        await response.Should()
            .HaveStatusCode(HttpStatusCode.ServiceUnavailable)
            .And.Match<HealthResult>(r => r.Status == "Unhealthy" && r.Entries.Any(x => x.Key == "database"));
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