using Application.Abstractions.Data;
using Domain.Products;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel;

namespace Application.Products.Commands;

internal sealed class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository;
    private readonly IApplicationDbContext _context;
    private readonly IDistributedCache _cache;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IApplicationDbContext context,
        IDistributedCache cache)
    {
        _productRepository = productRepository;
        _context = context;
        _cache = cache;
    }

    public async Task<Result<Guid>> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        bool exists =
            await _productRepository.ExistsByNameAsync(
                command.Name,
                cancellationToken);

        if (exists)
        {
            return Result.Failure<Guid>(
                Error.Conflict(
                    "Product.AlreadyExists",
                    "Product with same name already exists"));
        }

        Result<Product> productResult =
            Product.Create(
                command.Name,
                command.Description,
                command.Price,
                command.Stock,
                command.Category);

        if (productResult.IsFailure)
        {
            return Result.Failure<Guid>(
                productResult.Error);
        }

        Product product = productResult.Value;

        await _productRepository.AddAsync(product);

        await _context.SaveChangesAsync(
            cancellationToken);

        await _cache.RemoveAsync(
            "products:1:10:::",
            cancellationToken);

        return Result.Success(product.Id);
    }
}
