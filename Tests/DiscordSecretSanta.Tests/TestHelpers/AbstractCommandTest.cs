using FakeItEasy.Configuration;

namespace DiscordSecretSanta.Tests.TestHelpers;

public abstract class AbstractCommandTest<T>
{
    protected IDataStore DataStore;
    protected IMessages Messages = new EnglishMessages();
    protected ICampaignStatusService StatusService;
    protected T Command;

    [SetUp]
    public void SetupAbstract()
    {
        // Create this so that any un-faked calls throw an exception
        StatusService = A.Fake<ICampaignStatusService>(opts => opts.Strict());
        DataStore = A.Fake<IDataStore>();
        Command = InitCommand();
    }

    protected abstract T InitCommand();

    protected void ArrangeGetStatusReturns(CampaignStatusId status)
    {
        A.CallTo(() => DataStore.GetStatus(A<CancellationToken>.Ignored)).Returns(status);
    }

    protected void ArrangeGetMemberReturns(DiscordUserId id, SecretSantaMember member)
    {
        A.CallTo(() => DataStore.GetMember(A<DiscordUserId>.That.Matches(x => x.Value == id.Value), A<CancellationToken>._)).Returns(member);
    }

    protected IReturnValueArgumentValidationConfiguration<Task> AssertSetStatus(CampaignStatusId? expectedStatus=null)
    {
        if (expectedStatus.HasValue)
            return A.CallTo(() =>
            DataStore.SetStatus(A<CampaignStatusId>.That.Matches(x => x == expectedStatus), A<CancellationToken>.Ignored));
        
        return A.CallTo(() =>
            DataStore.SetStatus(A<CampaignStatusId>._, A<CancellationToken>.Ignored));
    }
}