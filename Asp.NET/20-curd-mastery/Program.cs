using _20_curd_mastery.Models;
using _20_curd_mastery.Auth;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var key = "super_secret_key_123456789_super_secret";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSingleton(new TokenService(key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/signup", async (User user, AppDbContext db, TokenService tokenService) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();

    var token = tokenService.Generate(user.Username);
    // var token = tokenService.Generate(user.Username, user.Role);

    return Results.Ok(new { token });
});

app.MapPost("/login", async (User login, AppDbContext db, TokenService tokenService) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u =>
        u.Username == login.Username &&
        u.Password == login.Password);

    if (user is null)
        return Results.Unauthorized();

    var token = tokenService.Generate(user.Username);

    // var token = tokenService.Generate(user.Username, user.Role);

    return Results.Ok(new { token });
});
app.MapGet("/admin", () => "Hello Admin!")
   .RequireAuthorization(policy => policy.RequireRole("Admin"));

app.MapGet("/dashboard", () => "User Dashboard")
   .RequireAuthorization(policy => policy.RequireRole("User", "Admin"));
var group = app.MapGroup("/api/todo");

var todos = new Todo();

group.MapGet("/", async (AppDbContext db) =>
{
    var AllTodos = await db.Todos.ToListAsync();
    return Results.Ok(AllTodos);
}).RequireAuthorization();

group.MapGet("/{id}", async (int id, AppDbContext db) =>
{

    var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == id);
    if (todo is null) return Results.NotFound();

    return Results.Ok(todo);

});

group.MapPut("/{id}", async (int id, Todo updatedTodo, AppDbContext db) =>
{
    var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == id);
    if (todo is null) return Results.NotFound();

    todo.Name = updatedTodo.Name;
    await db.SaveChangesAsync();

    return Results.Ok(todo);
});

group.MapDelete("/{id}", async (int id, AppDbContext db) =>
{
    var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == id);
    if (todo is null) return Results.NotFound();

    db.Todos.Remove(todo);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


group.MapPost("/", async (int id, string name, AppDbContext db) =>
{
    var TodoItem = new Todo
    {
        Id = id,
        Name = name
    };
    db.Todos.Add(TodoItem);
    await db.SaveChangesAsync();

    return Results.Created();
});
app.UseHttpsRedirection();

app.Run();
