using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;
using Microsoft.VisualBasic;

namespace DiscordSecretSanta.Tests.Commands;

public class OpenCommandTests
{
    private DataStore _dataStore;
    private Messages _messages = new EnglishMessages();
    private OpenCommand _command;

    [SetUp]
    public void Setup()
    {
        _dataStore = A.Fake<DataStore>();
        A.CallTo(() => _dataStore.GetNumberOfMembers(A<CancellationToken>.Ignored)).Returns(0);
        _command = new OpenCommand(_dataStore, _messages);
    }
    
    [Test]
    public async Task NotConfigured()
    {
        // ARRANGE
        A.CallTo(() => _dataStore.GetStatus(A<CancellationToken>.Ignored)).Returns(Status.NotConfigured);
        A.CallTo(() => _dataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(new SecretSantaConfig()
        {
            MaxPrice = string.Empty
        });
        
        // ACT
        var result = await _command.Handle(CancellationToken.None);

        // ASSERT
        result.ShouldBe([_messages.OpenNotConfigured(), _messages.MustHaveMaxPrice()]);
        
        A.CallTo(() => _dataStore.SetStatus(A<Status>._, A<CancellationToken>.Ignored)).MustNotHaveHappened();
    }

    [Test]
    public async Task NotConfiguredButActuallyIs()
    {
        // ARRANGE
        A.CallTo(() => _dataStore.GetStatus(A<CancellationToken>.Ignored)).Returns(Status.NotConfigured);
        A.CallTo(() => _dataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(TestConstants.ValidConfig());
        
        // ACT
        await _command.Handle(CancellationToken.None);

        // ASSERT
        A.CallTo(() => _dataStore.SetStatus(Status.Open, A<CancellationToken>.Ignored)).MustHaveHappened();
    }

    [Test]
    public async Task IsConfigured()
    {
        // ARRANGE
        A.CallTo(() => _dataStore.GetStatus(A<CancellationToken>.Ignored)).Returns(Status.Ready);
        A.CallTo(() => _dataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(TestConstants.ValidConfig());
        
        // ACT
        var result = await _command.Handle(CancellationToken.None);
        
        // ASSERT
        result.ShouldBe([_messages.NowOpen()]);
        A.CallTo(() => _dataStore.SetStatus(Status.Open, A<CancellationToken>.Ignored)).MustHaveHappened();
    }

    [TestCaseSource(typeof(TestData), nameof(TestData.TestCases))]
    public async Task CannotBeOpenedBecauseOfWrongStatus(Status status, string expectedMessage)
    {
        // ARRANGE
        A.CallTo(() => _dataStore.GetStatus(A<CancellationToken>.Ignored)).Returns(status);
        A.CallTo(() => _dataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(TestConstants.ValidConfig());
        
        // ACT
        var result = await _command.Handle(CancellationToken.None);
        
        // ASSERT
        result.ShouldBe([expectedMessage]);
        A.CallTo(() => _dataStore.SetStatus(Status.Open, A<CancellationToken>.Ignored)).MustNotHaveHappened();
    }
    
    private class TestData
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(Status.Drawn, new EnglishMessages().AlreadyDrawn());
                yield return new TestCaseData(Status.Open, new EnglishMessages().AlreadyOpen());
            }
        }
    }
}