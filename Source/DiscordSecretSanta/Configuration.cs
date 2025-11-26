using Microsoft.Extensions.Configuration;

namespace DiscordSecretSanta;

public class Configuration
{
    [ConfigurationKeyName("DiscordSecretSanta_Token")]
    public string Token { get; set; }
}