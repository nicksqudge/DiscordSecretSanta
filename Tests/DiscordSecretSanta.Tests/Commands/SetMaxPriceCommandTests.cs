using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Permissions;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class SetMaxPriceCommandTests : AbstractCommandTest<SetMaxPriceCommand>
{
    private ICanSetMaxPrice _permission;
    
    protected override SetMaxPriceCommand InitCommand()
    {
        _permission = A.Fake<ICanSetMaxPrice>();
        return new SetMaxPriceCommand(DataStore, Messages, _permission);
    }

    [Test]
    public async Task DoesNotHavePermission()
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser();
        A.CallTo(() => _permission.Can(A<InputUser>._,  A<CancellationToken>._)).Returns(false);

        // ACT
        var result = await Command.Handle(requestingUser, "£10", CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.YouAreNotAnAdmin());
        A.CallTo(() => DataStore.SetMaxPrice(A<string>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    public async Task EmptyString(string maxPrice)
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser();
        A.CallTo(() => _permission.Can(A<InputUser>._,  A<CancellationToken>._)).Returns(true);

        // ACT
        var result = await Command.Handle(requestingUser, maxPrice, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.MaxPriceMustHaveAValue());
        A.CallTo(() => DataStore.SetMaxPrice(A<string>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Theory]
    [TestCase("£10", Status.Open)]
    [TestCase("$10", Status.NotConfigured)]
    [TestCase("Whatever you want", Status.NotConfigured)]
    public async Task HappyPath(string maxPrice, Status status)
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser();
        A.CallTo(() => _permission.Can(A<InputUser>._,  A<CancellationToken>._)).Returns(true);
        A.CallTo(() => DataStore.GetStatus(A<CancellationToken>._)).Returns(status);

        // ACT
        var result = await Command.Handle(requestingUser, maxPrice, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.MaxPriceSaved());
        A.CallTo(() => DataStore.SetMaxPrice(A<string>.That.Matches(x => x == maxPrice), A<CancellationToken>._)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task AlreadyDrawn()
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser();
        A.CallTo(() => _permission.Can(A<InputUser>._,  A<CancellationToken>._)).Returns(true);
        A.CallTo(() => DataStore.GetStatus(A<CancellationToken>._)).Returns(Status.Drawn);

        // ACT
        var result = await Command.Handle(requestingUser, "$30", CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.AlreadyDrawn());
        A.CallTo(() => DataStore.SetMaxPrice(A<string>._, A<CancellationToken>._)).MustNotHaveHappened();
    }
}