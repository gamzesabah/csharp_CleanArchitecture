using Application.Abstractions.Data;
using Application.Products.Dtos;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel;
using System.Globalization;
using System.Text.Json;

namespace Application.Products.Queries;

internal sealed class GetProductListQueryHandler
    : IRequestHandler<
        GetProductListQuery,
        Result<PagedResult<ProductDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDistributedCache _cache;

    public GetProductListQueryHandler(
        IApplicationDbContext context,
        IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<PagedResult<ProductDto>>> Handle(
        GetProductListQuery query,
        CancellationToken cancellationToken)
    {
        string cacheKey =
            $"products:{query.Page}:{query.PageSize}:{query.Category}:{query.Search}:{query.SortBy}";

        string? cachedData =
            await _cache.GetStringAsync(
                cacheKey,
                cancellationToken);

        if (!string.IsNullOrWhiteSpace(cachedData))
        {
            PagedResult<ProductDto>? cachedResult =
                JsonSerializer.Deserialize<PagedResult<ProductDto>>(
                    cachedData);

            if (cachedResult is not null)
            {
                return Result.Success(cachedResult);
            }
        }

        IQueryable<Product> productsQuery =
            _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            productsQuery =
                productsQuery.Where(
                    x => x.Category == query.Category);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            productsQuery =
                productsQuery.Where(
                    x => x.Name.Contains(query.Search));
        }


        productsQuery =
            query.SortBy?.ToUpperInvariant() switch
        {
            "PRICE" =>
                productsQuery.OrderBy(x => x.Price),

            "NAME" =>
                productsQuery.OrderBy(x => x.Name),

            _ =>
                productsQuery.OrderBy(x => x.Name)
        };

        int totalCount =
            await productsQuery.CountAsync(
                cancellationToken);

        List<ProductDto> items =
            await productsQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Stock = x.Stock,
                    Category = x.Category
                })
                .ToListAsync(cancellationToken);

        var result = new PagedResult<ProductDto>(
            items,
            totalCount,
            query.Page,
            query.PageSize);

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(result),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(5)
            },
            cancellationToken);

        return Result.Success(result);
    }
}
