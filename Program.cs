using Microsoft.AspNetCore.Mvc;
using MongoDb;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<MongoDBService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var user = app.MapGroup("/users");

user.MapGet("", (MongoDBService mongoDBService) =>
{
    return mongoDBService.GetUsers();
})
.WithName("getUsers")
.WithOpenApi();

user.MapGet("/{id}", (MongoDBService mongoDBService, string id) =>
{
    return mongoDBService.GetUser(id);
})
.WithName("getUser")
.WithOpenApi();

user.MapPost("", async (MongoDBService mongoDBService, [FromBody] User user) =>
{
    await mongoDBService.CreateUser(user);
    return Results.Created($"/user/{user.Id}", user);
})
.WithName("createUser")
.WithOpenApi();

app.Run();