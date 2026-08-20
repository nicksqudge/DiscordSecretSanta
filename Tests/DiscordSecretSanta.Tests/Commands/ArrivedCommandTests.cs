using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class ArrivedCommandTests : AbstractCommandTest<ArrivedCommand>
{
    protected override ArrivedCommand InitCommand()
    {
        A.CallTo(() => StatusService.CanDoArrived(A<CampaignStatusId>._)).Returns(true);
        return new ArrivedCommand(DataStore, Messages, StatusService);
    }

    [TestCase(SecretSantaStatus.Arrived)]
    public async Task NotAlreadyArrived(SecretSantaStatus status)
    {
        // ARRANGE
        var sender = TestFactory.DiscordUserId();
        var receiver = TestFactory.DiscordUserId();
        ArrangeGetStatusReturns(CampaignStatusId.Drawn);
        ArrangeGetMembersSecretSanta(receiver, new SecretSantaMember(sender, TestFactory.WishlistUrl())
        {
            SecretSantaId = receiver,
            SecretSantaStatus = status
        });
        
        // ACT
        var (response, directMessage) = await Command.Handle(receiver, CancellationToken.None);
        
        // ASSERT
        response.ToString().ShouldBe(Messages.AlreadyArrived());
        directMessage.ShouldBeNull();
    }

    [TestCase(SecretSantaStatus.Pending)]
    [TestCase(SecretSantaStatus.Sent)]
    [TestCase(null)]
    public async Task Sends(SecretSantaStatus? status)
    {
        // ARRANGE
        var sender = TestFactory.DiscordUserId();
        var receiver = TestFactory.DiscordUserId();
        ArrangeGetStatusReturns(CampaignStatusId.Drawn);
        ArrangeGetMembersSecretSanta(receiver, new SecretSantaMember(sender, TestFactory.WishlistUrl())
        {
            SecretSantaId = receiver,
            SecretSantaStatus = status
        });
        
        // ACT
        var (response, directMessage) = await Command.Handle(receiver, CancellationToken.None);
        
        // ASSERT
        A.CallTo(() => StatusService.CanDoArrived(A<CampaignStatusId>._))
            .MustHaveHappened();
        A.CallTo(() => DataStore.SetSecretSantaStatus(A<DiscordUserId>.That.Matches(x => x == sender),
                A<SecretSantaStatus>.That.Matches(x => x == SecretSantaStatus.Arrived), A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        response.ToString().ShouldBe(Messages.MarkedAsArrived());
        directMessage.ShouldNotBeNull();
        directMessage.Sender.ShouldBe(sender);
    }

    private void ArrangeGetMembersSecretSanta(DiscordUserId targetUser, SecretSantaMember result)
    {
        A.CallTo(() => DataStore.GetMembersSecretSanta(A<DiscordUserId>.That.Matches(x => x == targetUser), A<CancellationToken>._))
            .Returns(result);
    }
}