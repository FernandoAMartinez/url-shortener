using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
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
            var response = await repository.CreateShortenUrl(req);

            if (response is null)
                return Results.BadRequest();

            return Results.Ok(response);
        });

        app.MapGet("/s/{shortenCode}",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromServices] IUrlRepository repository, [FromRoute] string shortenCode) =>
        {
            var response = await repository.GetOriginalUrl(shortenCode);
            
            if(string.IsNullOrEmpty(response.Url))
                return Results.BadRequest();

            return Results.Redirect(response.Url);
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