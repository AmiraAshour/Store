using FluentValidation;
using Store.Core.DTO.Order;

public class OrderDTOValidator : AbstractValidator<OrderDTO>
{
    public OrderDTOValidator()
    {
        RuleFor(x => x.basketId)
            .NotEmpty().WithMessage("BasketId is required.");

        RuleFor(x => x.deliveryMethodId)
            .GreaterThan(0).WithMessage("DeliveryMethodId must be valid.");

        RuleFor(x => x.shipAddress)
            .NotNull().WithMessage("Shipping address is required.");
    }
}