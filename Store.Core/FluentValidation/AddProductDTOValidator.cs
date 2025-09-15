using FluentValidation;
using Store.Core.DTO.Product;

public class AddProductDTOValidator : AbstractValidator<AddProductDTO>
{
    public AddProductDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.NewPrice).GreaterThan(0);
    }
}