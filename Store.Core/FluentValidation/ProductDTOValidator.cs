using FluentValidation;
using Store.Core.DTO.ProductDTO;

public class ProductDTOValidator : AbstractValidator<ProductDTO>
{
    public ProductDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500);

        RuleFor(x => x.NewPrice)
            .GreaterThan(0).WithMessage("New price must be greater than zero.");

        RuleFor(x => x.OldPrice)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required.");

        RuleFor(x => x.Photos)
            .NotNull();
    }
}