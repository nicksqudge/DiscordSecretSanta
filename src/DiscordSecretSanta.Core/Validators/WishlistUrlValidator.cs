using FluentValidation;

namespace DiscordSecretSanta.Core.Validators;

public class WishlistUrlValidator : AbstractValidator<string>
{
    public WishlistUrlValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .NotNull()
            .Must(x => x.Contains("amazon."));
    }
}