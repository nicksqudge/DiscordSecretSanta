namespace DiscordSecretSanta.Tests.TestHelpers;

public abstract class AbstractCommandTest<T>
{
    protected IDataStore DataStore;
    protected IMessages Messages = new EnglishMessages();
    protected T Command;

    [SetUp]
    public void Setup()
    {
        DataStore = A.Fake<IDataStore>();
        Command = InitCommand();
    }

    protected abstract T InitCommand();
}