using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class ArrivedCommandTests : AbstractCommandTest<ArrivedCommand>
{
    protected override ArrivedCommand InitCommand()
        => new(DataStore, Messages);

    [TestCase(Status.Open)]
    [TestCase(Status.NotConfigured)]
    [TestCase(Status.Ready)]
    [TestCase(Status.Closed)]
    public async Task NotRightCampaignStatus(Status status)
    {
        // ARRANGE
        ArrangeGetStatusReturns(status);
        
        // ACT
        var (response, directMessage) = await Command.Handle(TestFactory.DiscordUserId(), CancellationToken.None);
        
        // ASSERT
        response.ToString().ShouldBe(Messages.StatusNotValidForArrived());
        directMessage.ShouldBeNull();
    }

    [TestCase(SecretSantaStatus.Arrived)]
    public async Task NotAlreadyArrived(SecretSantaStatus status)
    {
        // ARRANGE
        var sender = TestFactory.DiscordUserId();
        var receiver = TestFactory.DiscordUserId();
        ArrangeGetStatusReturns(Status.Drawn);
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
        ArrangeGetStatusReturns(Status.Drawn);
        ArrangeGetMembersSecretSanta(receiver, new SecretSantaMember(sender, TestFactory.WishlistUrl())
        {
            SecretSantaId = receiver,
            SecretSantaStatus = status
        });
        
        // ACT
        var (response, directMessage) = await Command.Handle(receiver, CancellationToken.None);
        
        // ASSERT
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