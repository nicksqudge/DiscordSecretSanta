using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordSecretSanta.Tests;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Action<IServiceCollection>? _setup;

    public TestApplicationFactory(Action<IServiceCollection> setup)
    {
        _setup = setup;
    }

    public TestApplicationFactory()
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config => { });

        builder.ConfigureTestServices(services =>
        {
            services.AddTransient<ILoggerFactory, NullLogFactory>();
            _setup?.Invoke(services);
        });

        builder.UseEnvironment("Development");
    }
}