using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class SendCommandTests : AbstractCommandTest<SentCommand>
{
    protected override SentCommand InitCommand()
        => new(DataStore, Messages);

    [TestCase(Status.Open)]
    [TestCase(Status.NotConfigured)]
    [TestCase(Status.Ready)]
    public async Task NotRightStatus(Status status)
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
    [TestCase(SecretSantaStatus.Received)]
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
        response.ToString().ShouldBe(Messages.MarkedAsSent());
        directMessage.ShouldNotBeNull();
        directMessage.Receiver.ShouldBe(receiver);
    }
}