using DiscordSecretSanta.Permissions;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Permissions;

public class CanSetMaxPriceTests
{
    private IDataStore _dataStore;
    private IPermission _subject;

    [SetUp]
    public void SetUp()
    {
        _dataStore = A.Fake<IDataStore>();
        _subject = new CanSetMaxPrice(_dataStore);
    }
    
    [Test]
    public async Task IsServerAdmin()
    {
        // ARRANGE
        var user = TestFactory.InputUser(true);

        // ACT
        var result = await _subject.Can(user, CancellationToken.None);
        
        // ASSERT
        result.ShouldBeTrue();
        A.CallTo(() => _dataStore.IsAdminInConfig(A<DiscordUserId>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Test]
    public async Task IsAdmin()
    {
        // ARRANGE
        var user = TestFactory.InputUser();
        A.CallTo(() => _dataStore.IsAdminInConfig(A<DiscordUserId>._, A<CancellationToken>._))
            .Returns(true);
        
        // ACT
        var result = await _subject.Can(user, CancellationToken.None);
        
        // ASSERT
        result.ShouldBeTrue();
    }

    [Test]
    public async Task IsNotAnyAdmin()
    {
        // ARRANGE
        var user = TestFactory.InputUser();
        A.CallTo(() => _dataStore.IsAdminInConfig(A<DiscordUserId>._, A<CancellationToken>._))
            .Returns(false);
        
        // ACT
        var result = await _subject.Can(user, CancellationToken.None);
        
        // ASSERT
        result.ShouldBeFalse();
    }
}