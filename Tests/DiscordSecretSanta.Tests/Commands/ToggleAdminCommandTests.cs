using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class ToggleAdminCommandTests : AbstractCommandTest<ToggleAdminCommand>
{
    [Test]
    public async Task AGuildAdmin()
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser();
        var targetUser = TestFactory.InputUser(isAdmin: true);
        ArrangeUserIsAdmin(requestingUser, true);
        ArrangeUserIsAdmin(targetUser, false);

        // ACT
        var result = await Command.Handle(targetUser, requestingUser, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.IsGuidAdmin(targetUser.Name));
    }

    [Test]
    public async Task AlreadyAnAdmin()
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser();
        var targetUser = TestFactory.InputUser();
        ArrangeUserIsAdmin(requestingUser, true);
        ArrangeUserIsAdmin(targetUser, true);

        // ACT
        var result = await Command.Handle(targetUser, requestingUser, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.IsNoLongerAnAdmin(targetUser.Name));
        A.CallTo(() => DataStore.ToggleAdmin(A<DiscordUserId>._, A<bool>.That.Matches(x => x == false), A<CancellationToken>._))
            .MustHaveHappened();
    }

    [Test]
    public async Task NotAnAdmin()
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser();
        var targetUser = TestFactory.InputUser();
        ArrangeUserIsAdmin(requestingUser, true);
        ArrangeUserIsAdmin(targetUser, false);

        // ACT
        var result = await Command.Handle(targetUser, requestingUser, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.IsNowAnAdmin(targetUser.Name));
        A.CallTo(() => DataStore.ToggleAdmin(A<DiscordUserId>._, A<bool>.That.Matches(x => x == true), A<CancellationToken>._))
            .MustHaveHappened();
    }

    [Test]
    public async Task RequesterIsntAnAdmin()
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser();
        var targetUser = TestFactory.InputUser();
        ArrangeUserIsAdmin(requestingUser, false);
        ArrangeUserIsAdmin(targetUser, false);

        // ACT
        var result = await Command.Handle(targetUser, requestingUser, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.YouDoNotHavePermissionToMakeAdmin());
        A.CallTo(() => DataStore.ToggleAdmin(A<DiscordUserId>._, A<bool>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [SetUp]
    public void Setup()
    {
        A.CallTo(() => DataStore.ToggleAdmin(A<DiscordUserId>._, A<bool>._, A<CancellationToken>._)).Returns(Task.CompletedTask);
    }

    private void ArrangeUserIsAdmin(InputUser user, bool result)
    {
        A.CallTo(() => DataStore.IsAdmin(A<DiscordUserId>.That.Matches(x => x == user.Id), A<CancellationToken>._)).Returns(result);
    }

    protected override ToggleAdminCommand InitCommand()
        => new(DataStore, Messages);
}