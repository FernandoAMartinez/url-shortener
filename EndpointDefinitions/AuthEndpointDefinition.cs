using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Contracts;
using UrlShortener.Extensions;
using UrlShortener.Helpers;
using UrlShortener.Repositories;

namespace UrlShortener.EndpointDefinitions;

public class AuthEndpointDefinition : IEndpointDefinition
{
    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
    }

    public void RegisterEndpoints(WebApplication app)
    {
        var endpoint = app.MapGroup("/api/auth").WithTags("Auth");

        endpoint.MapPost("/login", [AllowAnonymous]
        async ([FromBody] LoginRequest req, IAuthRepository repository) =>
        {
            var jwtHelper = new JWTHelper(Environment.GetEnvironmentVariable("JWT_SECRET"));

            var user = await repository.Login(req);

            if (user is null)
                return Results.NotFound();

            var token = jwtHelper.CreateToken(user.Email);

            return Results.Ok(new LoginResponse(user.Id, token));
        });

        endpoint.MapPost("/register", [AllowAnonymous]
        async ([FromBody] RegisterRequest req, IAuthRepository repository) =>
        {
            var user = await repository.Register(req);

            if (user is null)
                return Results.BadRequest();

            return Results.Ok(new UserResponse(user.Id, user.Email, user.Username, user.CreatedAt, user.UpdatedAt));
        });
    }
}