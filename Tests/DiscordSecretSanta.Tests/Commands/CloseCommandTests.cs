using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Permissions;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class CloseCommandTests : AbstractCommandTest<CloseCommand, CloseCommand.Input, CloseCommand.Output>
{
    private ICanClose _permission;
    
    protected override CloseCommand InitCommand()
    {
        _permission = A.Fake<ICanClose>();
        return new CloseCommand(DataStore, Messages, _permission);
    }

    [Test]
    public async Task NotRightCampaignStatus()
    {
        // ARRANGE
        ArrangeHasPermission(true);
        
        // ACT
        await AssertShouldOnlyAllowStatus(new CloseCommand.Input(TestFactory.InputUser()), CampaignStatusId.Drawn,
            CampaignStatusId.Open);
        
        // ASSERT
        AssertSetStatus(CampaignStatusId.Closed).MustNotHaveHappened();
    }

    [TestCase(CampaignStatusId.Drawn)]
    [TestCase(CampaignStatusId.Open)]
    public async Task OnlyAdminsCanClose(CampaignStatusId validStatus)
    {
        // ARRANGE
        ArrangeGetStatusReturns(validStatus);
        ArrangeHasPermission(false);
        
        // ACT
        var response = await Command.Handle(new CloseCommand.Input(TestFactory.InputUser(isServerAdmin: false)), CancellationToken.None);
        
        // ASSERT
        response.Reply.ToString().ShouldBe(Messages.YouAreNotAnAdmin());
        AssertSetStatus(CampaignStatusId.Closed).MustNotHaveHappened();
    }

    [TestCase(CampaignStatusId.Drawn, true)]
    [TestCase(CampaignStatusId.Open, false)]
    public async Task HappyPath(CampaignStatusId status, bool isServerAdmin)
    {
        // ARRANGE
        ArrangeGetStatusReturns(status);
        ArrangeHasPermission(true);
        
        // ACT
        var response = await Command.Handle(new CloseCommand.Input(TestFactory.InputUser(isServerAdmin)), CancellationToken.None);
        
        // ASSERT
        response.Reply.ToString().ShouldBe(Messages.CampaignClosed());
        AssertSetStatus(CampaignStatusId.Closed).MustHaveHappened();
    }

    private void ArrangeHasPermission(bool permission)
    {
        A.CallTo(() => _permission.Can(A<InputUser>._,  A<CancellationToken>._)).Returns(permission);
    }
}