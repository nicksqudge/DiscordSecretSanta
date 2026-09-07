using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class SentCommandTests : AbstractCommandTest<SentCommand, SentCommand.Input, SentCommand.Output>
{
    protected override SentCommand InitCommand()
        => new(DataStore, Messages);

    [Test]
    public async Task NotRightCampaignStatus()
    {
        await AssertShouldOnlyAllowStatus(new SentCommand.Input(TestFactory.DiscordUserId()), CampaignStatusId.Drawn);
    }

    [TestCase(SecretSantaStatus.Sent)]
    [TestCase(SecretSantaStatus.Arrived)]
    public async Task NotAlreadySent(SecretSantaStatus status)
    {
        // ARRANGE
        var input = new SentCommand.Input(TestFactory.DiscordUserId());
        var receiver = TestFactory.DiscordUserId();
        ArrangeGetStatusReturns(CampaignStatusId.Drawn);
        ArrangeGetMemberReturns(input.RequestingUserId, new SecretSantaMember(input.RequestingUserId, TestFactory.WishlistUrl())
        {
            SecretSantaId = receiver,
            SecretSantaStatus = status
        });
        
        // ACT
        var response = await Command.Handle(input, CancellationToken.None);
        
        // ASSERT
        response.Reply.ToString().ShouldBe(Messages.AlreadySent());
        response.ToSend.ShouldBeNull();
    }

    [TestCase(SecretSantaStatus.Pending)]
    [TestCase(null)]
    public async Task Sends(SecretSantaStatus? status)
    {
        // ARRANGE
        var input = new SentCommand.Input(TestFactory.DiscordUserId());
        var receiver = TestFactory.DiscordUserId();
        ArrangeGetStatusReturns(CampaignStatusId.Drawn);
        ArrangeGetMemberReturns(input.RequestingUserId, new SecretSantaMember(input.RequestingUserId, TestFactory.WishlistUrl())
        {
            SecretSantaId = receiver,
            SecretSantaStatus = status
        });
        
        // ACT
        var response = await Command.Handle(input, CancellationToken.None);
        
        // ASSERT
        A.CallTo(() => DataStore.SetSecretSantaStatus(A<DiscordUserId>.That.Matches(x => x == input.RequestingUserId),
                A<SecretSantaStatus>.That.Matches(x => x == SecretSantaStatus.Sent), A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        response.Reply.ToString().ShouldBe(Messages.MarkedAsSent());
        response.ToSend.ShouldNotBeNull();
        response.ToSend.Receiver.ShouldBe(receiver);
    }
}