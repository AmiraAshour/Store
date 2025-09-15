using FluentValidation;
using Store.Core.DTO.Account;

public class AddressDTOValidator : AbstractValidator<AddressDTO>
{
    public AddressDTOValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.City)
            .NotEmpty();

        RuleFor(x => x.ZipCode)
            .NotEmpty();

        RuleFor(x => x.Street)
            .NotEmpty();

        RuleFor(x => x.State)
            .NotEmpty();
    }
}