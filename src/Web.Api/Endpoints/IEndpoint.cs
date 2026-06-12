using Application.Products.Commands;
using MediatR;
using SharedKernel;
using Web.Api.Endpoints;

namespace Web.Api.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
