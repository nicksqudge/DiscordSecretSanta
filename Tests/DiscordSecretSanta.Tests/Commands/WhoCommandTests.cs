using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class WhoCommandTests : AbstractCommandTest<WhoCommand>
{
    protected override WhoCommand InitCommand()
        => new WhoCommand();

    [TestCase(Status.NotConfigured)]
    [TestCase(Status.Open)]
    [TestCase(Status.Ready)]
    public async Task GivenStatus_ShouldSayCannotShowValue(Status status)
    {
        // ARRANGE
        ArrangeGetStatusReturns(status);
        var requestingUser = TestFactory.InputUser();
        
        // ACT
        var (result, directMessage) = await Command.Handle(requestingUser, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.CouldNotShowWho());
        directMessage.ShouldNotBeNull();
    }

    [Test]
    public async Task CampaignIsDrawn_Returns()
    {
        // ARRANGE
        ArrangeGetStatusReturns(Status.Drawn);
        var requestingUser = TestFactory.InputUser();
        var secretSanta = TestFactory.DiscordUserId();
        A.CallTo(() => DataStore.GetMember(requestingUser.Id, CancellationToken.None))
            .ReturnsLazily(() => new SecretSantaMember(secretSanta, TestFactory.WishlistUrl()));

        // ACT
        var (result, directMessage) = await Command.Handle(requestingUser, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.CouldShow());
        directMessage.secretSanta.ShouldBe(secretSanta);
        directMessage.targetUserId.ShouldBe(requestingUser.Id);
    }
}