using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Contracts;
using UrlShortener.Extensions;
using UrlShortener.Repositories;

namespace UrlShortener.EndpointDefinitions;

public class UrlEndpointDefinition : IEndpointDefinition
{
    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IUrlRepository, UrlRepository>();
    }

    public void RegisterEndpoints(WebApplication app)
    {
        var endpoint = app.MapGroup("api/url").WithTags("Url Shortener");

        endpoint.MapPost("/shorten",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromServices] IUrlRepository repository, [FromBody] UrlShortenedRequest req) =>
        {
            try
            {
                await repository.CreateShortenUrl(req);
            }
            catch (Exception)
            {
                return Results.BadRequest();
            }

            return Results.Ok();
        });

        endpoint.MapPost("/test/",
        [AllowAnonymous] async ([FromServices] IUrlRepository repository, [FromBody] string url) =>
        {
            var result = repository.TestUrlShortener(url);

            if (string.IsNullOrEmpty(result))
                return Results.BadRequest();

            return Results.Ok(result);
        });
    }
}