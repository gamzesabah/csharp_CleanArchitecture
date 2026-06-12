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
