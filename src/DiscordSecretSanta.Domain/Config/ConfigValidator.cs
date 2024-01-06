using FluentValidation;

namespace DiscordSecretSanta.Domain.Config;

public class ConfigValidator : AbstractValidator<AppConfig>
{
    public ConfigValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x)
            .Must(x => x != new AppConfig())
            .WithErrorCode(ConfigErrors.InvalidConfig);
    }
}