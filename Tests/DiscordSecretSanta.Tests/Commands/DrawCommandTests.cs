using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Permissions;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class DrawCommandTests : AbstractCommandTest<DrawCommand>
{
    private Dictionary<DiscordUserId, DiscordUserId> _secretSantas = new();
    private List<SecretSantaMember> _members = new();
    
    protected override DrawCommand InitCommand() => new(DataStore, Messages, new CanStartDraw());

    [TestCase(10)]
    [TestCase(100)]
    [TestCase(1000)]
    public async Task ShouldAssignPeopleSecretSantas(int numberOfMembers)
    {
        // ARRANGE
        ArrangeGetStatusReturns(CampaignStatusId.Open);
        ArrangeNumberOfMembers(numberOfMembers);
        var requestingUser = TestFactory.InputUser(isServerAdmin: true);
        
        // ACT
        var (result, directMessages) = await Command.Handle(requestingUser, CancellationToken.None);

        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.DrawComplete());
        
        A.CallTo(() => DataStore.SetSecretSanta(A<DiscordUserId>._, A<DiscordUserId>._, CancellationToken.None))
            .MustHaveHappened(numberOfMembers, Times.Exactly);
        AssertSetStatus(CampaignStatusId.Drawn).MustHaveHappened();
        
        _secretSantas.Count.ShouldBe(numberOfMembers);
        
        foreach (var member in _members)
        {
            _secretSantas.ContainsKey(member.UserId).ShouldBe(true);
            _secretSantas.Any(x => x.Key == member.UserId && x.Value == member.UserId).ShouldBeFalse();
            _secretSantas.Count(x => x.Value == member.UserId).ShouldBe(1);
            directMessages.Any(dm => dm.TargetUserId == member.UserId).ShouldBe(true);
        }
    }

    [Theory]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task ShouldHaveAtLeast3People(int members)
    {
        // ARRANGE
        ArrangeGetStatusReturns(CampaignStatusId.Open);
        ArrangeNumberOfMembers(members);
        var requestingUser = TestFactory.InputUser(isServerAdmin: true);
        
        // ACT
        var (result, directMessages) = await Command.Handle(requestingUser, CancellationToken.None);
        
        // ASSERT
        result.ToString().Trim().ShouldContain(Messages.CouldNotDraw());
        AssertDidNotDraw(directMessages);
    }

    [TestCase(CampaignStatusId.Drawn)]
    [TestCase(CampaignStatusId.NotConfigured)]
    [TestCase(CampaignStatusId.Ready)]
    [TestCase(CampaignStatusId.Closed)]
    public async Task GivenStatus_ShouldNotDraw(CampaignStatusId startingStatus)
    {
        // ARRANGE
        ArrangeGetStatusReturns(startingStatus);
        var requestingUser = TestFactory.InputUser(isServerAdmin: true);
        
        // ACT
        var (result, directMessages) = await Command.Handle(requestingUser, CancellationToken.None);
        
        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.CouldNotDraw());
        AssertDidNotDraw(directMessages);
    }

    [Test]
    public async Task UserDoesNotHavePermission()
    {
        // ARRANGE
        var requestingUser = TestFactory.InputUser(isServerAdmin: false);
        
        // ACT
        var (result, directMessages) = await Command.Handle(requestingUser, CancellationToken.None);
        
        // ASSERT
        result.ToString().Trim().ShouldBe(Messages.YouDoNotHavePermissionToDraw());
        AssertDidNotDraw(directMessages);
    }

    private void ArrangeNumberOfMembers(int members)
    {
        _members = new();
        _secretSantas = new();
        for (var i = 0; i < members; i++)
        {
            _members.Add(new SecretSantaMember(TestFactory.DiscordUserId(), TestFactory.WishlistUrl()));
        }
        
        A.CallTo(() => DataStore.GetNumberOfMembers(A<CancellationToken>._)).Returns(members);
        A.CallTo(() => DataStore.GetMembers(A<CancellationToken>._)).Returns(_members.ToArray());

        A.CallTo(() => DataStore.SetSecretSanta(A<DiscordUserId>._, A<DiscordUserId>._, CancellationToken.None))
            .Invokes((DiscordUserId target, DiscordUserId secretSanta, CancellationToken _) =>
            {
                _secretSantas.Add(target, secretSanta);
            });
    }

    private void AssertDidNotDraw(DrawCommand.DirectMessage[] messages)
    {
        AssertSetStatus(CampaignStatusId.Drawn).MustNotHaveHappened();
        A.CallTo(() => DataStore.SetSecretSanta(A<DiscordUserId>._, A<DiscordUserId>._, CancellationToken.None)).MustNotHaveHappened();
        _secretSantas.Count.ShouldBe(0);
        messages.Length.ShouldBe(0);
    }
}