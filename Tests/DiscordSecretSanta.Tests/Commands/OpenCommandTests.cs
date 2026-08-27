using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class OpenCommandTests : AbstractCommandTest<OpenCommand, OpenCommand.Input, OpenCommand.Output>
{
    [SetUp]
    public void Setup()
    {
        A.CallTo(() => DataStore.GetNumberOfMembers(A<CancellationToken>.Ignored)).Returns(0);
    }

    protected override OpenCommand InitCommand()
        => new (DataStore, Messages);

    [Test]
    public async Task NotConfigured()
    {
        // ARRANGE
        ArrangeGetStatusReturns(CampaignStatusId.NotConfigured);
        A.CallTo(() => DataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(new SecretSantaConfig()
        {
            MaxPrice = string.Empty
        });
        
        // ACT
        var result = await Command.Handle(new OpenCommand.Input(), CancellationToken.None);

        // ASSERT
        result.Reply.ToString().ShouldBe(ViaStringBuilder(Messages.OpenNotConfigured(), Messages.MustHaveMaxPrice()));
        
        AssertSetStatus().MustNotHaveHappened();
    }

    [Test]
    public async Task NotConfiguredButActuallyIs()
    {
        // ARRANGE
        ArrangeGetStatusReturns(CampaignStatusId.NotConfigured);
        A.CallTo(() => DataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(TestConstants.ValidConfig());
        
        // ACT
        await Command.Handle(new OpenCommand.Input(), CancellationToken.None);

        // ASSERT
        AssertSetStatus(CampaignStatusId.Open).MustHaveHappened();
    }

    [Test]
    public async Task IsConfigured()
    {
        // ARRANGE
        ArrangeGetStatusReturns(CampaignStatusId.Ready);
        A.CallTo(() => DataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(TestConstants.ValidConfig());
        
        // ACT
        var result = await Command.Handle(new OpenCommand.Input(), CancellationToken.None);
        
        // ASSERT
        result.Reply.ToString().ShouldBe(ViaStringBuilder(Messages.NowOpen()));
        AssertSetStatus(CampaignStatusId.Open).MustHaveHappened();
    }
    
    public async Task CannotBeOpenedBecauseOfWrongStatus(CampaignStatusId status, string expectedMessage)
    {
        // ARRANGE
        A.CallTo(() => DataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(TestConstants.ValidConfig());
        
        // ACT
        await AssertShouldOnlyAllowStatus(new  OpenCommand.Input(), status);
        
        // ASSERT
        AssertSetStatus(CampaignStatusId.Open).MustNotHaveHappened();
    }

    private string ViaStringBuilder(params string[] input)
    {
        return input.ToStringBuilder().ToString();
    }
}