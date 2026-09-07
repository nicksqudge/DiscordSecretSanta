using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class StatusCommandTests : AbstractCommandTest<StatusCommand, StatusCommand.Input, StatusCommand.Output>
{
    [SetUp]
    public void Setup()
    {
        A.CallTo(() => DataStore.GetNumberOfMembers(A<CancellationToken>.Ignored)).Returns(0);
    }

    protected override StatusCommand InitCommand()
     => new  (DataStore, Messages);

    [TestCaseSource(typeof(TestData), nameof(TestData.TestCases))]
    public async Task NotOpenOrDrawn(CampaignStatusId status, string expectedResult, bool expectShowMaxPrice)
    {
        // ARRANGE
        var maxPrice = "£10";
        A.CallTo(() => DataStore.GetConfig(A<CancellationToken>.Ignored))
            .Returns(new SecretSantaConfig()
            {
                MaxPrice = maxPrice
            });
        A.CallTo(() => DataStore.GetStatus(A<CancellationToken>.Ignored))
            .Returns(status);

        // ACT
        var result = await Command.Handle(new StatusCommand.Input(), CancellationToken.None);

        // ASSERT
        result.Reply.ToString().ShouldContain(expectedResult);

        if (expectShowMaxPrice)
        {
            result.Reply.ToString().ShouldContain(Messages.StatusMaxPrice(maxPrice));
        }
        else
        {
            result.Reply.ToString().ShouldNotContain(Messages.StatusMaxPrice(maxPrice));
        }
    }
    
    private class TestData
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(CampaignStatusId.NotConfigured, new EnglishMessages().StatusIsNotConfigured(), false);
                yield return new TestCaseData(CampaignStatusId.Ready, new EnglishMessages().StatusIsReady(), true);
                yield return new TestCaseData(CampaignStatusId.Drawn, new EnglishMessages().StatusIsDrawn(), true);
                yield return new TestCaseData(CampaignStatusId.Open, new EnglishMessages().StatusIsOpen(0), true);
                yield return new TestCaseData(CampaignStatusId.Closed, new EnglishMessages().StatusIsClosed(), false);
            }
        }
    }
}