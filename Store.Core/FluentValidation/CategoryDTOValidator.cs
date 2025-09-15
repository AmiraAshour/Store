using FluentValidation;
using Store.Core.DTO.CategoryEntityDTO;

public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
{
    public CategoryDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .MaximumLength(250);
    }
}