using Application.Products.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Products.Queries;

public sealed record GetProductListQuery(
    int Page = 1,
    int PageSize = 10,
    string? Category = null,
    string? Search = null,
    string? SortBy = null)
    : IRequest<Result<PagedResult<ProductDto>>>;
