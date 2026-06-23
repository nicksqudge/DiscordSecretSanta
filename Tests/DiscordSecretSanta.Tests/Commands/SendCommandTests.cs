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
        var result = await Command.Handle(CancellationToken.None);
        
        // ASSERT
        result.ToString().ShouldBe(Messages.StatusNotValidForSent());
    }

    [Test]
    public async Task NotAlreadySent()
    {
        // ARRANGE
        ArrangeGetStatusReturns(Status.Drawn);
        
        // ACT
        
        // ASSERT
    }

    [Test]
    public async Task Sends()
    {
        
    }
}