using Microsoft.AspNetCore.Mvc;

namespace MongoDb;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var user = app.MapGroup("/users");

        user.MapGet("", (MongoDBService mongoDBService, [FromQuery] int page = 1, [FromQuery] int size = 10) =>
        {
            return mongoDBService.GetUsers(page, size);
        })
        .CacheOutput(c => c.Expire(TimeSpan.FromMinutes(1)))
        .WithName("getUsers")
        .WithTags("Get All Users")
        .WithOpenApi();

        user.MapGet("/{id}", async (MongoDBService mongoDBService, string id) =>
        {
            var user = await mongoDBService.GetUser(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        })
        .CacheOutput(c => c.Expire(TimeSpan.FromMinutes(1)))
        .WithName("getUser")
        .WithTags("Get User By Id")
        .WithOpenApi();

        user.MapPost("", async (MongoDBService mongoDBService, [FromBody] User user) =>
        {
            await mongoDBService.CreateUser(user);
            return Results.Created($"/user/{user.Id}", user);
        })
        .WithName("createUser")
        .WithTags("User Create")
        .WithOpenApi();

        user.MapPut("/{id}", async (MongoDBService mongoDBService, string id, [FromBody] User user) =>
        {
            await mongoDBService.UpdateUser(id, user);
            return Results.Ok();
        })
        .WithName("updateUser")
        .WithTags("User Update")
        .WithOpenApi();

        user.MapDelete("/{id}", async (MongoDBService mongoDBService, string id) =>
        {
            await mongoDBService.DeleteUser(id);
            return Results.NoContent();
        })
        .WithName("deleteUser")
        .WithTags("User Delete")
        .WithOpenApi();
    }
}
