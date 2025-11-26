using FluentValidation;

namespace DiscordSecretSanta;

public class SecretSantaConfig
{
    public string MaxPrice { get; set; } = string.Empty;
}

public class SecretSantaConfigValidator : AbstractValidator<SecretSantaConfig>
{
    public SecretSantaConfigValidator(Messages messages)
    {
        RuleFor(x => x.MaxPrice).NotEmpty().WithMessage(messages.MustHaveMaxPrice());
    }
}