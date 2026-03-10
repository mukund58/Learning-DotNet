using Microsoft.EntityFrameworkCore;
using revEFCore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PostDbContext>();

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PostDbContext>();
    db.Database.EnsureCreated();
}

// ---- User Endpoints ----

app.MapGet("/api/users", async (PostDbContext db) =>
    await db.Users.ToListAsync());

app.MapGet("/api/users/{id}", async (int id, PostDbContext db) =>
    await db.Users.FindAsync(id) is User user
        ? Results.Ok(user)
        : Results.NotFound());

app.MapPost("/api/users", async (User user, PostDbContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/api/users/{user.UserId}", user);
});

app.MapPut("/api/users/{id}", async (int id, User inputUser, PostDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    user.Name = inputUser.Name;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/users/{id}", async (int id, PostDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ---- Post Endpoints ----

app.MapGet("/api/posts", async (PostDbContext db) =>
    await db.Posts.Include(p => p.User).ToListAsync());

app.MapGet("/api/posts/{id}", async (int id, PostDbContext db) =>
    await db.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id) is Post post
        ? Results.Ok(post)
        : Results.NotFound());

app.MapPost("/api/posts", async (Post post, PostDbContext db) =>
{
    db.Posts.Add(post);
    await db.SaveChangesAsync();
    return Results.Created($"/api/posts/{post.Id}", post);
});

app.MapPut("/api/posts/{id}", async (int id, Post inputPost, PostDbContext db) =>
{
    var post = await db.Posts.FindAsync(id);
    if (post is null) return Results.NotFound();

    post.Title = inputPost.Title;
    post.PostDate = inputPost.PostDate;
    post.UserId = inputPost.UserId;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/posts/{id}", async (int id, PostDbContext db) =>
{
    var post = await db.Posts.FindAsync(id);
    if (post is null) return Results.NotFound();

    db.Posts.Remove(post);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
