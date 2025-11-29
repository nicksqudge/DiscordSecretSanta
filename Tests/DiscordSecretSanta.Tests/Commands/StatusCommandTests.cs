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
    public async Task NotOpenOrDrawn(Status status, string expectedResult)
    {
        // ARRANGE
        A.CallTo(() => DataStore.GetStatus(A<CancellationToken>.Ignored))
            .Returns(status);

        // ACT
        var result = await Command.Handle(CancellationToken.None);

        // ASSERT
        result.ShouldBe(expectedResult);
    }
    
    private class TestData
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(Status.NotConfigured, new EnglishMessages().StatusIsNotConfigured());
                yield return new TestCaseData(Status.Ready, new EnglishMessages().StatusIsReady());
                yield return new TestCaseData(Status.Drawn, new EnglishMessages().StatusIsDrawn());
                yield return new TestCaseData(Status.Open, new EnglishMessages().StatusIsOpen(0));
            }
        }
    }
}