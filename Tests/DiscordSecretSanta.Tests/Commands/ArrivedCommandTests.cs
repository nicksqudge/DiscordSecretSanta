using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class ArrivedCommandTests : AbstractCommandTest<ArrivedCommand, ArrivedCommand.Input, ArrivedCommand.Response>
{
    protected override ArrivedCommand InitCommand() => new(DataStore, Messages);

    [Test]
    public async Task OnlySupportsDrawn()
    {
        await AssertShouldOnlyAllowStatus(new ArrivedCommand.Input(TestFactory.DiscordUserId()), CampaignStatusId.Drawn);
    }

    [TestCase(SecretSantaStatus.Arrived)]
    public async Task NotAlreadyArrived(SecretSantaStatus status)
    {
        // ARRANGE
        var sender = TestFactory.DiscordUserId();
        var receiver = new ArrivedCommand.Input(TestFactory.DiscordUserId());
        ArrangeGetStatusReturns(CampaignStatusId.Drawn);
        ArrangeGetMembersSecretSanta(receiver.RequestingUserId, new SecretSantaMember(sender, TestFactory.WishlistUrl())
        {
            SecretSantaId = receiver.RequestingUserId,
            SecretSantaStatus = status
        });
        
        // ACT
        var response = await Command.Handle(receiver, CancellationToken.None);
        
        // ASSERT
        response.Output.ToString().ShouldBe(Messages.AlreadyArrived());
        response.DirectMessageTo.ShouldBeNull();
    }

    [TestCase(SecretSantaStatus.Pending)]
    [TestCase(SecretSantaStatus.Sent)]
    [TestCase(null)]
    public async Task Sends(SecretSantaStatus? status)
    {
        // ARRANGE
        var sender = TestFactory.DiscordUserId();
        var receiver = new ArrivedCommand.Input(TestFactory.DiscordUserId());
        ArrangeGetStatusReturns(CampaignStatusId.Drawn);
        ArrangeGetMembersSecretSanta(receiver.RequestingUserId, new SecretSantaMember(sender, TestFactory.WishlistUrl())
        {
            SecretSantaId = receiver.RequestingUserId,
            SecretSantaStatus = status
        });
        
        // ACT
        var response = await Command.Handle(receiver, CancellationToken.None);
        
        // ASSERT
        A.CallTo(() => DataStore.SetSecretSantaStatus(A<DiscordUserId>.That.Matches(x => x == sender),
                A<SecretSantaStatus>.That.Matches(x => x == SecretSantaStatus.Arrived), A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        response.Output.ToString().ShouldBe(Messages.MarkedAsArrived());
        response.DirectMessageTo.ShouldNotBeNull();
        response.DirectMessageTo.Sender.ShouldBe(sender);
    }

    private void ArrangeGetMembersSecretSanta(DiscordUserId targetUser, SecretSantaMember result)
    {
        A.CallTo(() => DataStore.GetMembersSecretSanta(A<DiscordUserId>.That.Matches(x => x == targetUser), A<CancellationToken>._))
            .Returns(result);
    }
}