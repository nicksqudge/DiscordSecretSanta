using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class SentCommandTests : AbstractCommandTest<SentCommand>
{
    protected override SentCommand InitCommand()
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
        response.ToString().ShouldBe(Messages.StatusNotValidForSent());
        directMessage.ShouldBeNull();
    }

    [TestCase(SecretSantaStatus.Sent)]
    [TestCase(SecretSantaStatus.Arrived)]
    public async Task NotAlreadySent(SecretSantaStatus status)
    {
        // ARRANGE
        var sender = TestFactory.DiscordUserId();
        var receiver = TestFactory.DiscordUserId();
        ArrangeGetStatusReturns(Status.Drawn);
        ArrangeGetMemberReturns(sender, new SecretSantaMember(sender, TestFactory.WishlistUrl())
        {
            SecretSantaId = receiver,
            SecretSantaStatus = status
        });
        
        // ACT
        var (response, directMessage) = await Command.Handle(sender, CancellationToken.None);
        
        // ASSERT
        response.ToString().ShouldBe(Messages.AlreadySent());
        directMessage.ShouldBeNull();
    }

    [TestCase(SecretSantaStatus.Pending)]
    [TestCase(null)]
    public async Task Sends(SecretSantaStatus? status)
    {
        // ARRANGE
        var sender = TestFactory.DiscordUserId();
        var receiver = TestFactory.DiscordUserId();
        ArrangeGetStatusReturns(Status.Drawn);
        ArrangeGetMemberReturns(sender, new SecretSantaMember(sender, TestFactory.WishlistUrl())
        {
            SecretSantaId = receiver,
            SecretSantaStatus = status
        });
        
        // ACT
        var (response, directMessage) = await Command.Handle(sender, CancellationToken.None);
        
        // ASSERT
        A.CallTo(() => DataStore.SetSecretSantaStatus(A<DiscordUserId>.That.Matches(x => x == sender),
                A<SecretSantaStatus>.That.Matches(x => x == SecretSantaStatus.Sent), A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        response.ToString().ShouldBe(Messages.MarkedAsSent());
        directMessage.ShouldNotBeNull();
        directMessage.Receiver.ShouldBe(receiver);
    }
}