using FluentValidation;
using Store.Core.DTO;

public class ReviewDTOValidator : AbstractValidator<AddReviewDTO>
{
    public ReviewDTOValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0);

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5);

        RuleFor(x => x.Comment)
            .MaximumLength(500);
    }
}