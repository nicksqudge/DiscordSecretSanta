using DiscordSecretSanta.Commands;
using FakeItEasy.Configuration;

namespace DiscordSecretSanta.Tests.TestHelpers;

public abstract class AbstractCommandTest<T, TInput, TResponse> 
    where T : AbstractCommand<TInput, TResponse> 
    where TInput : class, ICommandInput
    where TResponse : class, ICommandResponse, new ()
{
    protected IDataStore DataStore;
    protected IMessages Messages = new EnglishMessages();
    protected T Command;

    [SetUp]
    public void SetupAbstract()
    {
        // Create this so that any un-faked calls throw an exception
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

    protected async Task AssertShouldOnlyAllowStatus(TInput input, params CampaignStatusId[] expectedStatus)
    {
        var statuses = Enum.GetValues(typeof(CampaignStatusId)).Cast<CampaignStatusId>().ToList();
        foreach (var status in statuses)
        {
            if (expectedStatus.Contains(status))
                continue;
            
            var response = await Command.Handle(input, CancellationToken.None);
            response.Output.ShouldNotBeNull();
            response.Output.ShouldBe(Messages.StatusNotSupported());
        }
    }
}