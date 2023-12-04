using System.Net;
using DiscordSecretSanta.Configure.HealthChecks;
using DiscordSecretSanta.Domain.HealthCheck;
using DiscordSecretSanta.Domain.Integrations;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Tests.Controllers;

public class HealthCheckController : ApiTestFixture
{
    [Test]
    public async Task AllWorking()
    {
        // ARRANGE
        using var api = CreateClient(services =>
        {
            services.AddDatabaseHealthCheck<AllGoodHealthCheck>();
            services.AddTransient<IDiscordStatusApi, AllSystemsOperational>();
        });

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
        using var api = CreateClient(services =>
        {
            services.AddDatabaseHealthCheck<NoDatabaseConnectionHealthCheck>();
            services.AddTransient<IDiscordStatusApi, AllSystemsOperational>();
        });

        // ACT
        var response = await api.GetAsync(DiscordSecretSanta.Controllers.HealthCheckController.HealthRoute);

        // ASSERT
        await response.Should()
            .HaveStatusCode(HttpStatusCode.ServiceUnavailable)
            .And.Match<HealthResult>(r => r.Status == "Unhealthy" &&
                                          r.Entries.Any(x => x.Key == "database"));
    }

    [Test]
    public async Task DiscordIsDown()
    {
        // ARRANGE
        using var api = CreateClient(services =>
        {
            services.AddDatabaseHealthCheck<AllGoodHealthCheck>();
            services.AddTransient<IDiscordStatusApi, PartialOutage>();
        });

        // ACT
        var response = await api.GetAsync(DiscordSecretSanta.Controllers.HealthCheckController.HealthRoute);

        // ASSERT
        await response.Should()
            .HaveStatusCode(HttpStatusCode.ServiceUnavailable)
            .And.Match<HealthResult>(r => r.Status == "Unhealthy" &&
                                          r.Entries.Any(x =>
                                              x.Key == "discord" && x.Value.Description == "Partial Outage"));
    }

    private class NoDatabaseConnectionHealthCheck : IDatabaseHealthChecks
    {
        public Task<bool> CanConnectToDatabase()
        {
            return Task.FromResult(false);
        }
    }

    private class AllGoodHealthCheck : IDatabaseHealthChecks
    {
        public Task<bool> CanConnectToDatabase()
        {
            return Task.FromResult(true);
        }
    }

    private class AllSystemsOperational : IDiscordStatusApi
    {
        public Task<string> GetStatus()
        {
            return Task.FromResult("All Systems Operational");
        }
    }

    private class PartialOutage : IDiscordStatusApi
    {
        public Task<string> GetStatus()
        {
            return Task.FromResult("Partial Outage");
        }
    }
}