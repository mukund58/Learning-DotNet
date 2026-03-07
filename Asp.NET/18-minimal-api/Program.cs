using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DotNetEnv;
using _18_minimal_api.Models;
using _18_minimal_api.Auth;

Env.Load(); // Load environment variables from .env file
var key = Environment.GetEnvironmentVariable("KEY");
// var key = "super_key_123456789";";
// var key = "this_is_a_super_secret_key_123456";


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(new TokenService(key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var users = new List<User>();
var todos = new List<TodoItem>();



var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/", () => "Hello World!");

app.MapGet("/secure", () => "Authorized!")
   .RequireAuthorization();

   app.MapPost("/signup", (string username, string password, TokenService tokenService) =>
   {
       if (username != "admin" || password != "admin")
           return Results.Unauthorized();

       var token = tokenService.Generate(username);

       users.Add(new User(username, password));

       return Results.Ok(new { token });
   });

   app.MapPost("/login", (string username, string password, TokenService tokenService) =>
   {
       if (username != "admin" || password != "admin")
           return Results.Unauthorized();

       var token = tokenService.Generate(username);

       return Results.Ok(new { token });
   });

app.MapGet("/todo", () => Results.Ok(todos));

app.MapGet("/todo/{id:int}", (int id) =>
{
    if (id < 0)
        return Results.BadRequest("Invalid ID");

    if (id >= todos.Count)
        return Results.NotFound("Item Not Found");

        var todo = todos[id];
        return Results.Ok(todo);
});

app.MapPost("/todo", (string data, TokenService tokenService) =>
{
    if (string.IsNullOrWhiteSpace(data))
        return Results.BadRequest("Invalid item");

    if (todos.Contains(data))
        return Results.BadRequest("Item already exists");

    var todo = new TodoItem(Guid.NewGuid().ToString(), data);
    todos.Add(todo);
    return Results.Created($"/todo/{todos.Count - 1}", todo);
});

app.MapPut("/todo/{id:int}", (int id, string data, TokenService tokenService) =>
{
    if (string.IsNullOrWhiteSpace(data))
        return Results.BadRequest("Invalid item");

    if (id < 0 || id >= todos.Count)
        return Results.NotFound("Item Not Found");

    todos[id] = new TodoItem(todos[id].Id, data);
    return Results.NoContent();
});

app.MapDelete("/todo/{id:int}", (int id) =>
{
    if (id < 0 || id >= todos.Count)
        return Results.NotFound("Item Not Found");

    todos.RemoveAt(id);
    return Results.NoContent();
});

app.Run();
