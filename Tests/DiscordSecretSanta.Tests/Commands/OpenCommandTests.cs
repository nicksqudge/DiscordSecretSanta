using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

public class OpenCommandTests : AbstractCommandTest<OpenCommand>
{
    [SetUp]
    public void Setup()
    {
        A.CallTo(() => DataStore.GetNumberOfMembers(A<CancellationToken>.Ignored)).Returns(0);
    }

    protected override OpenCommand InitCommand()
        => new (DataStore, Messages);

    [Test]
    public async Task NotConfigured()
    {
        // ARRANGE
        ArrangeGetStatusReturns(Status.NotConfigured);
        A.CallTo(() => DataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(new SecretSantaConfig()
        {
            MaxPrice = string.Empty
        });
        
        // ACT
        var result = await Command.Handle(CancellationToken.None);

        // ASSERT
        result.ToString().ShouldBe(ViaStringBuilder(Messages.OpenNotConfigured(), Messages.MustHaveMaxPrice()));
        
        AssertSetStatus().MustNotHaveHappened();
    }

    [Test]
    public async Task NotConfiguredButActuallyIs()
    {
        // ARRANGE
        ArrangeGetStatusReturns(Status.NotConfigured);
        A.CallTo(() => DataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(TestConstants.ValidConfig());
        
        // ACT
        await Command.Handle(CancellationToken.None);

        // ASSERT
        AssertSetStatus(Status.Open).MustHaveHappened();
    }

    [Test]
    public async Task IsConfigured()
    {
        // ARRANGE
        ArrangeGetStatusReturns(Status.Ready);
        A.CallTo(() => DataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(TestConstants.ValidConfig());
        
        // ACT
        var result = await Command.Handle(CancellationToken.None);
        
        // ASSERT
        result.ToString().ShouldBe(ViaStringBuilder(Messages.NowOpen()));
        AssertSetStatus(Status.Open).MustHaveHappened();
    }

    [TestCaseSource(typeof(TestData), nameof(TestData.TestCases))]
    public async Task CannotBeOpenedBecauseOfWrongStatus(Status status, string expectedMessage)
    {
        // ARRANGE
        ArrangeGetStatusReturns(status);
        A.CallTo(() => DataStore.GetConfig(A<CancellationToken>.Ignored)).Returns(TestConstants.ValidConfig());
        
        // ACT
        var result = await Command.Handle(CancellationToken.None);
        
        // ASSERT
        result.ToString().ShouldBe(ViaStringBuilder(expectedMessage));
        AssertSetStatus(Status.Open).MustNotHaveHappened();
    }

    private string ViaStringBuilder(params string[] input)
    {
        return input.ToStringBuilder().ToString();
    }
    
    private class TestData
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(Status.Drawn, new EnglishMessages().AlreadyDrawn());
                yield return new TestCaseData(Status.Open, new EnglishMessages().AlreadyOpen());
                yield return new TestCaseData(Status.Closed, new EnglishMessages().StatusIsClosed());
            }
        }
    }
}