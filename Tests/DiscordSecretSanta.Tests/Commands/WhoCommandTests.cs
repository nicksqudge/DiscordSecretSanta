using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class WhoCommandTests : AbstractCommandTest<WhoCommand, WhoCommand.Input, WhoCommand.Output>
{
    protected override WhoCommand InitCommand()
        => new (DataStore, Messages);
    
    [Test]
    public async Task OnlyViableDuringDrawn()
    {
        await AssertShouldOnlyAllowStatus(new WhoCommand.Input(TestFactory.InputUser()), CampaignStatusId.Drawn);
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
        var response = await Command.Handle(new WhoCommand.Input(requestingUser), CancellationToken.None);

        // ASSERT
        response.Reply.ToString().Trim().ShouldBe(Messages.CouldShow());
        response.Who.ShouldNotBeNull();
        response.Who.SecretSantaId.ShouldBe(secretSanta);
        response.Who.WhoAskedId.ShouldBe(requestingUser.Id);
        response.Who.SecretSantaWishlist.ShouldNotBeNull();
    }
}