using FakeItEasy.Configuration;

namespace DiscordSecretSanta.Tests.TestHelpers;

public abstract class AbstractCommandTest<T>
{
    protected IDataStore DataStore;
    protected IMessages Messages = new EnglishMessages();
    protected T Command;

    [SetUp]
    public void SetupAbstract()
    {
        DataStore = A.Fake<IDataStore>();
        Command = InitCommand();
    }

    protected abstract T InitCommand();

    protected void ArrangeGetStatusReturns(Status status)
    {
        A.CallTo(() => DataStore.GetStatus(A<CancellationToken>.Ignored)).Returns(status);
    }

    protected void ArrangeGetMemberReturns(DiscordUserId id, SecretSantaMember member)
    {
        A.CallTo(() => DataStore.GetMember(A<DiscordUserId>.That.Matches(x => x.Value == id.Value), A<CancellationToken>._)).Returns(member);
    }

    protected IReturnValueArgumentValidationConfiguration<Task> AssertSetStatus(Status? expectedStatus=null)
    {
        if (expectedStatus.HasValue)
            return A.CallTo(() =>
            DataStore.SetStatus(A<Status>.That.Matches(x => x == expectedStatus), A<CancellationToken>.Ignored));
        
        return A.CallTo(() =>
            DataStore.SetStatus(A<Status>._, A<CancellationToken>.Ignored));
    }
}