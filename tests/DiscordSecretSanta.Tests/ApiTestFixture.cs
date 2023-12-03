using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Tests;

[TestFixture]
public abstract class ApiTestFixture
{
    protected HttpClient CreateClient()
    {
        var application = new TestApplicationFactory();
        return application.CreateClient();
    }

    protected HttpClient CreateClient(Action<IServiceCollection> setupServices)
    {
        var application = new TestApplicationFactory(setupServices);
        return application.CreateClient();
    }
}