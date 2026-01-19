using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;
namespace DiscordSecretSanta.Tests.Commands;

public class JoinCommandTests : AbstractCommandTest<JoinCommand>
{
    private readonly DiscordUserId _targetUserId = TestFactory.DiscordUserId();
    private readonly List<IWishlistUrlValidator> _validators = new();
    private const string ValidWishlistUrl = "https://www.amazon.co.uk/hz/wishlist/ls/35GHJDXGIAUDIAK?ref_=wl_share";
    
    [SetUp]
    public void Setup()
    {
        
    }

    protected override JoinCommand InitCommand()
        => new(DataStore, Messages, _validators);

    [TestCaseSource(typeof(StatusTestCaseData), nameof(StatusTestCaseData.StatusThatAreNotOpen))]
    public async Task NotOpen(Status status)
    {
        // ARRANGE
        ArrangeGetStatusReturns(status);
        
        // ACT
        var result = await Command.Handle(_targetUserId, "https://amazon.com", CancellationToken.None);
        
        // ASSERT
        result.ToString().ShouldBe(Messages.NotOpenForJoining());
        A.CallTo(() => DataStore.AddMember(A<DiscordUserId>._, A<Uri>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Test]
    public async Task AllValidatorsReturnFalseToValidWishlistUrl()
    {
        // ARRANGE
        ArrangeGetStatusReturns(Status.Open);
        ArrangeValidatorReturns(false);
        ArrangeValidatorReturns(false);
        
        // ACT
        var result = await Command.Handle(_targetUserId, "anything", CancellationToken.None);
        
        // ASSERT
        result.ToString().ShouldBe(Messages.NotAValidWishlistUrl());
        A.CallTo(() => DataStore.AddMember(A<DiscordUserId>._, A<Uri>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Test]
    public async Task AlreadyAMember()
    {
        // ARRANGE
        ArrangeGetStatusReturns(Status.Open);
        ArrangeValidatorReturns(true);
        ArrangeHasMemberAlreadySignedUp(true);
        
        // ACT
        var result = await Command.Handle(_targetUserId, ValidWishlistUrl, CancellationToken.None);
        
        // ASSERT
        result.ToString().ShouldBe(Messages.YouHaveAlreadyJoined());
        A.CallTo(() => DataStore.AddMember(A<DiscordUserId>._, A<Uri>._, A<CancellationToken>._)).MustNotHaveHappened();
    }
    
    [Test]
    public async Task ValidUrl()
    {
        // ARRANGE
        ArrangeGetStatusReturns(Status.Open);
        ArrangeValidatorReturns(true);
        ArrangeValidatorReturns(false);
        ArrangeHasMemberAlreadySignedUp(false);
        
        // ACT
        var result = await Command.Handle(_targetUserId, ValidWishlistUrl, CancellationToken.None);
        
        // ASSERT
        result.ToString().ShouldBe(Messages.YouHaveSuccessfullyJoined());
        A.CallTo(() => DataStore.AddMember(A<DiscordUserId>._, A<Uri>._, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
    }

    private void ArrangeValidatorReturns(bool isValidResult)
    { 
        var validator = A.Fake<IWishlistUrlValidator>();
        A.CallTo(() => 
            validator.IsValid(A<string>._, A<CancellationToken>._))
            .Returns(isValidResult ? new Uri(ValidWishlistUrl) : null);
        _validators.Add(validator);
    }

    private void ArrangeHasMemberAlreadySignedUp(bool result)
    {
        A.CallTo(() =>
                DataStore
                    .GetMember(A<DiscordUserId>._, A<CancellationToken>._))
            .Returns(result ? new SecretSantaMember(TestFactory.DiscordUserId(), new Uri(ValidWishlistUrl)) : null);
    }
}