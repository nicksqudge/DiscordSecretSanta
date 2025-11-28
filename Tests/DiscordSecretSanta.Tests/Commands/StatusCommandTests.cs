using DiscordSecretSanta.Commands;

namespace DiscordSecretSanta.Tests.Commands;

public class StatusCommandTests
{
    private IDataStore _dataStore;
    private Messages _messages = new EnglishMessages();
    private StatusCommand _command;

    [SetUp]
    public void Setup()
    {
        _dataStore = A.Fake<IDataStore>();
        A.CallTo(() => _dataStore.GetNumberOfMembers(A<CancellationToken>.Ignored)).Returns(0);
        _command = new StatusCommand(_dataStore, _messages);
    }
    
    [TestCaseSource(typeof(TestData), nameof(TestData.TestCases))]
    public async Task NotOpenOrDrawn(Status status, string expectedResult)
    {
        // ARRANGE
        A.CallTo(() => _dataStore.GetStatus(A<CancellationToken>.Ignored))
            .Returns(status);

        // ACT
        var result = await _command.Handle(CancellationToken.None);

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