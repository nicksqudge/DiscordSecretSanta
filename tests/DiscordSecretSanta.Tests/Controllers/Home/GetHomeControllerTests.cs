﻿using DiscordSecretSanta.Controllers.Home;
using DiscordSecretSanta.Domain.Setup;

namespace DiscordSecretSanta.Tests.Controllers.Home;

public class GetHomeControllerTests : ApiTestFixture
{
    [Test]
    public async Task NoConfigSetup()
    {
        // ARRANGE
        using var api = CreateClient();

        // ACT
        var response = await api.GetAsync(GetHomeController.Route);

        // ASSERT
        await response.Should()
            .BeOk()
            .And.ContentMatches(new SetupResponse
            {
                User = null,
                ActiveCampaign = null,
                ConfigOkay = false,
                ConfigDetails = null
            });
    }

    [Test]
    public async Task ConfigIsSetupButThereAreSomeIssues()
    {
        // ARRANGE
        using var api = CreateClient();

        // ACT
        var response = await api.GetAsync(GetHomeController.Route);

        // ASSERT
        await response.Should()
            .BeOk()
            .And.ContentMatches(new SetupResponse
            {
                User = null,
                ActiveCampaign = null,
                ConfigOkay = false,
                ConfigDetails = new List<SetupConfigResponse>
                {
                    new()
                    {
                        Key = "database",
                        Reason = "cannot-connect",
                        IsHealthy = false
                    }
                }
            });
    }

    [Test]
    public async Task ConfigIsOkayButUserIsNotLoggedIn()
    {
        // ARRANGE
        using var api = CreateClient();

        // ACT
        var response = await api.GetAsync(GetHomeController.Route);

        // ASSERT
        await response.Should()
            .BeOk()
            .And.ContentMatches(new SetupResponse
            {
                User = null,
                ActiveCampaign = null,
                ConfigOkay = true,
                ConfigDetails = null
            });
    }

    [Test]
    public async Task UserIsLoggedInButIsNotAdmin()
    {
        // ARRANGE
        using var api = CreateClient();

        // ACT
        var response = await api.GetAsync(GetHomeController.Route);

        // ASSERT
        await response.Should()
            .BeOk()
            .And.ContentMatches(new SetupResponse
            {
                User = new SetupUserResponse
                {
                    Name = "Test",
                    IsAdmin = false
                },
                ActiveCampaign = null,
                ConfigOkay = true,
                ConfigDetails = null
            });
    }

    [Test]
    public async Task UserIsLoggedInAndIsAdmin()
    {
        // ARRANGE
        using var api = CreateClient();

        // ACT
        var response = await api.GetAsync(GetHomeController.Route);

        // ASSERT
        await response.Should()
            .BeOk()
            .And.ContentMatches(new SetupResponse
            {
                User = new SetupUserResponse
                {
                    Name = "Test",
                    IsAdmin = true
                },
                ActiveCampaign = null,
                ConfigOkay = true,
                ConfigDetails = null
            });
    }

    [Test]
    public async Task UserIsLoggedInAndHasCampaign()
    {
        // ARRANGE
        using var api = CreateClient();

        // ACT
        var response = await api.GetAsync(GetHomeController.Route);

        // ASSERT
        await response.Should()
            .BeOk()
            .And.ContentMatches(new SetupResponse
            {
                User = new SetupUserResponse
                {
                    Name = "Test",
                    IsAdmin = true
                },
                ActiveCampaign = new SetupCampaignResponse
                {
                    Name = "Test"
                },
                ConfigOkay = true,
                ConfigDetails = null
            });
    }
}