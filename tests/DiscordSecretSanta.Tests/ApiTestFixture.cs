namespace DiscordSecretSanta.Tests;

[TestFixture]
public abstract class ApiTestFixture
{
    protected HttpClient CreateClient()
    {
        var application = new TestApplicationFactory();
        return application.CreateClient();
    }
}