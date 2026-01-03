using FluentValidation;

namespace DiscordSecretSanta.Validators;

public class NotEmptyStringValidator : AbstractValidator<string>
{
    public NotEmptyStringValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
    }
}