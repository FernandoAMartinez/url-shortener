using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Contracts;
using UrlShortener.Extensions;
using UrlShortener.Repositories;

namespace UrlShortener.EndpointDefinitions;

public class UserEndpointDefinition : IEndpointDefinition
{
    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public void RegisterEndpoints(WebApplication app)
    {
        var endpoint = app.MapGroup("/api/users").WithTags("Users");

        endpoint.MapGet("/", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromServices] IUserRepository repository) =>
        {
            var users = await repository.GetUsers();

            if (users is null)
                return Results.NotFound();

            return Results.Ok(users);
        });

        endpoint.MapGet("/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromServices] IUserRepository repository, [FromRoute] long id) =>
        {
            var user = await repository.GetUserById(id);

            if (user is null)
                return Results.NotFound();

            return Results.Ok(user);
        });

        endpoint.MapPatch("/{id}/update", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromBody] UserUpdateRequest req, [FromServices] IUserRepository repository) =>
        {
            try
            {
                await repository.UpdateUser(req);
            }
            catch (Exception)
            {
                return Results.BadRequest();
            }


            return Results.Ok();
        });

        endpoint.MapDelete("/{id}/delete", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromServices] IUserRepository repository, [FromRoute] long id) =>
        {
            try
            {
                await repository.DeleteUser(id);
            }
            catch (Exception)
            {
                return Results.BadRequest();
            }
            return Results.Ok();
        });


        endpoint.MapPatch("/change-password", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromBody] ChangePasswordRequest req, [FromServices] IUserRepository repository) =>
        {
            var user = await repository.ChangePassword(req);

            if (user is null)
                return Results.BadRequest();

            return Results.Ok(new UserResponse(user.Id, user.Username, user.Email, user.CreatedAt, user.UpdatedAt));
        });

        endpoint.MapPost("/forgot-password", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromServices] IUserRepository repository) =>
        {
            //Nothing
        });

        endpoint.MapPost("/reset-password", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromServices] IUserRepository repository) =>
        {
            //Nothing
        });

        endpoint.MapPost("/verify-email", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async ([FromServices] IUserRepository repository) =>
        {
            //Nothing
        });
    }
}