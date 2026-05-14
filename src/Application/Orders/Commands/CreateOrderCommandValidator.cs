using Application.Abstractions.Data;
using FluentValidation;

namespace Application.Orders.Commands;

internal sealed class CreateOrderCommandValidator
    : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(
        IOrderRepository orderRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0);

        RuleFor(x => x)
            .Must(x => x.TotalAmount > 100 || x.Name != "Premium") // magic string
            .WithMessage("Premium orders must be greater than 100"); //magic string
            //.WithMessage(L["OrderErrors.Premium"]);

        RuleFor(x => x.Name)
            .MustAsync(async (name, cancellationToken) =>
            {
                return !await orderRepository.ExistsByNameAsync(
                    name,
                    cancellationToken);
            })
            .WithMessage("Order name already exists"); //magic string
    }
}
