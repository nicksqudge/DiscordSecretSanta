using DiscordSecretSanta.Domain.Config;
using FluentValidation.TestHelper;

namespace DiscordSecretSanta.Domain.Tests.Config;

public class ConfigValidatorTests
{
    private readonly ConfigValidator _target = new();

    [Test]
    public async Task DefaultConfigIsValid()
    {
        // ARRANGE
        var config = new AppConfig();

        // ACT
        var result = await _target.TestValidateAsync(config);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorCode(ConfigErrors.InvalidConfig);
    }
}