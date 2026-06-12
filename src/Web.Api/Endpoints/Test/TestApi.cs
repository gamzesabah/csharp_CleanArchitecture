using Web.Api.Endpoints;

namespace Web.Api.Endpoints.Test;

public sealed class TestApi : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "test-api",
            async (
                IHttpClientFactory factory) =>
            {
                try
                {
                    HttpClient client =
                        factory.CreateClient("TestApi");

                    HttpResponseMessage response =
                        await client.GetAsync("/posts");

                    string content =
                        await response.Content
                            .ReadAsStringAsync();

                    return Results.Ok(content);
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.ToString());
                }
            })
            .WithTags("Test");
    }
}
