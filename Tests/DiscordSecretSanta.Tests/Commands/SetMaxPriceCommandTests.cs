using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Permissions;
using DiscordSecretSanta.Tests.TestHelpers;
using NUnit.Framework.Internal;

namespace DiscordSecretSanta.Tests.Commands;

public class SetMaxPriceCommandTests : AbstractCommandTest<SetMaxPriceCommand, SetMaxPriceCommand.Input, SetMaxPriceCommand.Output>
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
        var result = await Command.Handle(new SetMaxPriceCommand.Input(requestingUser, "£10"), CancellationToken.None);

        // ASSERT
        result.Reply.ToString().Trim().ShouldBe(Messages.YouAreNotAnAdmin());
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
        var result = await Command.Handle(new SetMaxPriceCommand.Input(requestingUser, maxPrice), CancellationToken.None);

        // ASSERT
        result.Reply.ToString().Trim().ShouldBe(Messages.MaxPriceMustHaveAValue());
        A.CallTo(() => DataStore.SetMaxPrice(A<string>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Theory]
    [TestCase("£10", CampaignStatusId.Ready)]
    [TestCase("$10", CampaignStatusId.NotConfigured)]
    [TestCase("Whatever you want", CampaignStatusId.NotConfigured)]
    public async Task HappyPath(string maxPrice, CampaignStatusId status)
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser();
        A.CallTo(() => _permission.Can(A<InputUser>._,  A<CancellationToken>._)).Returns(true);
        ArrangeGetStatusReturns(status);

        // ACT
        var result = await Command.Handle(new SetMaxPriceCommand.Input(requestingUser, maxPrice), CancellationToken.None);

        // ASSERT
        result.Reply.ToString().Trim().ShouldBe(Messages.MaxPriceSaved());
        A.CallTo(() => DataStore.SetMaxPrice(A<string>.That.Matches(x => x == maxPrice), A<CancellationToken>._)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task OnlySupportsReadyOrNotConfigured()
    {
        await AssertShouldOnlyAllowStatus(new SetMaxPriceCommand.Input(TestFactory.InputUser(), "£40"),
            CampaignStatusId.NotConfigured, CampaignStatusId.Ready);
        A.CallTo(() => DataStore.SetMaxPrice(A<string>._, A<CancellationToken>._)).MustNotHaveHappened();
    }
}