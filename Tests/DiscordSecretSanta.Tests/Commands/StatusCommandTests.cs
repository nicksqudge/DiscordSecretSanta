using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class StatusCommandTests : AbstractCommandTest<StatusCommand>
{
    [SetUp]
    public void Setup()
    {
        A.CallTo(() => DataStore.GetNumberOfMembers(A<CancellationToken>.Ignored)).Returns(0);
    }

    protected override StatusCommand InitCommand()
     => new  (DataStore, Messages);

    [TestCaseSource(typeof(TestData), nameof(TestData.TestCases))]
    public async Task NotOpenOrDrawn(Status status, string expectedResult, bool expectShowMaxPrice)
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
        var result = await Command.Handle(CancellationToken.None);

        // ASSERT
        result.ToString().ShouldContain(expectedResult);

        if (expectShowMaxPrice)
        {
            result.ToString().ShouldContain(Messages.StatusMaxPrice(maxPrice));
        }
        else
        {
            result.ToString().ShouldNotContain(Messages.StatusMaxPrice(maxPrice));
        }
    }

    [TestCaseSource(typeof(StatusTestCaseData), nameof(StatusTestCaseData.AllStatuses))]
    public async Task ShowMaxPrice(Status status)
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
        var result = await Command.Handle(CancellationToken.None);

        // ASSERT
        if (status != Status.NotConfigured)
            result.ToString().ShouldContain(Messages.StatusMaxPrice(maxPrice));
        else
            result.ToString().ShouldNotContain(Messages.StatusMaxPrice(maxPrice));
    }
    
    private class TestData
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(Status.NotConfigured, new EnglishMessages().StatusIsNotConfigured(), false);
                yield return new TestCaseData(Status.Ready, new EnglishMessages().StatusIsReady(), true);
                yield return new TestCaseData(Status.Drawn, new EnglishMessages().StatusIsDrawn(), true);
                yield return new TestCaseData(Status.Open, new EnglishMessages().StatusIsOpen(0), true);
            }
        }
    }
}