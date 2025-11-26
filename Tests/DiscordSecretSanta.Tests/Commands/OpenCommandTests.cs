using DiscordSecretSanta.Commands;

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
        A.CallTo(() => _dataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(new SecretSantaConfig()
        {
            MaxPrice = "£15"
        });
        
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
        A.CallTo(() => _dataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(new SecretSantaConfig()
        {
            MaxPrice = "£15"
        });
        
        // ACT
        var result = await _command.Handle(CancellationToken.None);
        
        // ASSERT
        result.ShouldBe([_messages.NowOpen()]);
        A.CallTo(() => _dataStore.SetStatus(Status.Open, A<CancellationToken>.Ignored)).MustHaveHappened();
    }
}