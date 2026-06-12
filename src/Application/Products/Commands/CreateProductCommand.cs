using MediatR;
using SharedKernel;

namespace Application.Products.Commands;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string Category)
    : IRequest<Result<Guid>>;
