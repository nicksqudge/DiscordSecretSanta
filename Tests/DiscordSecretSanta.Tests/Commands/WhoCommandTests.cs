using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class WhoCommandTests : AbstractCommandTest<WhoCommand>
{
    protected override WhoCommand InitCommand()
        => new (DataStore, Messages);

    [TestCase(CampaignStatusId.NotConfigured)]
    [TestCase(CampaignStatusId.Open)]
    [TestCase(CampaignStatusId.Ready)]
    [TestCase(CampaignStatusId.Closed)]
    public async Task GivenStatus_ShouldSayCannotShowValue(CampaignStatusId status)
    {
        // ARRANGE
        ArrangeGetStatusReturns(status);
        var requestingUser = TestFactory.InputUser();
        
        // ACT
        var (result, directMessage) = await Command.Handle(requestingUser, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.CouldNotShowWho());
        directMessage.ShouldBeNull();
    }

    [Test]
    public async Task CampaignIsDrawn_Returns()
    {
        // ARRANGE
        ArrangeGetStatusReturns(CampaignStatusId.Drawn);
        var requestingUser = TestFactory.InputUser();
        var secretSanta = TestFactory.DiscordUserId();
        ArrangeGetMemberReturns(requestingUser.Id, new SecretSantaMember(requestingUser.Id, TestFactory.WishlistUrl())
        {
            SecretSantaId = secretSanta
        });
        ArrangeGetMemberReturns(secretSanta, new SecretSantaMember(secretSanta, TestFactory.WishlistUrl()));

        // ACT
        var (result, directMessage) = await Command.Handle(requestingUser, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.CouldShow());
        directMessage.SecretSantaId.ShouldBe(secretSanta);
        directMessage.WhoAskedId.ShouldBe(requestingUser.Id);
        directMessage.SecretSantaWishlist.ShouldNotBeNull();
    }
}