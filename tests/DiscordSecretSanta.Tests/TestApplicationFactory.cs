using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace DiscordSecretSanta.Tests;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config => { });
        builder.ConfigureTestServices(services => { });

        builder.UseEnvironment("Development");
    }
}