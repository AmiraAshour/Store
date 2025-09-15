using FluentValidation;
using Store.Core.DTO.Account;

public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
{
    public RegisterDTOValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty().MinimumLength(6);

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required.");
    }
}