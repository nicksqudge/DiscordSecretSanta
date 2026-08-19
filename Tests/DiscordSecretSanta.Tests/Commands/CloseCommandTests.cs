using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class CloseCommandTests : AbstractCommandTest<CloseCommand>
{
    protected override CloseCommand InitCommand()
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
        var response = await Command.Handle(TestFactory.DiscordUserId(), CancellationToken.None);
        
        // ASSERT
        response.ToString().ShouldBe(Messages.StatusNotValidForSent());
        
    }
}