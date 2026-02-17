using Microsoft.EntityFrameworkCore;
using MinimalApi.Data;
using MinimalApi.Models;
using MinimalApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MinimalApi.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MinimalApi.Security;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// DB connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")
    )
);

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

// JWT Authentication
builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddScoped<JwtTokenService>();

Console.WriteLine("JWT CONFIG CHECK:");
Console.WriteLine($"Key: {builder.Configuration["Jwt:Key"]}");
Console.WriteLine($"Issuer: {builder.Configuration["Jwt:Issuer"]}");
Console.WriteLine($"Audience: {builder.Configuration["Jwt:Audience"]}");

// Build app
var app = builder.Build();

// Middleware
app.UseAuthentication();
app.UseAuthorization();

// Routes
app.MapGet("/", () => "API with PostgreSQL 🚀");

// Get users (protected)
app.MapGet("/users", async (AppDbContext db) =>
    await db.Users.Select(u => new UserResponseDto
    {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email,
        CreatedAt = u.CreatedAt
    }).ToListAsync()
).RequireAuthorization();

// Register
app.MapPost("/auth/register", async (
    RegisterDto dto,
    AppDbContext db
) =>
{
    if (await db.Users.AnyAsync(u => u.Email == dto.Email))
        return Results.BadRequest("Email already exists");

    var user = new User
    {
        Name = dto.Name,
        Email = dto.Email,
        PasswordHash = PasswordHasher.Hash(dto.Password)
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok();
});

// Login
app.MapPost("/auth/login", async (
    LoginDto dto,
    AppDbContext db,
    JwtTokenService jwt
) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

    if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
        return Results.Unauthorized();

    var token = jwt.CreateToken(user);
    return Results.Ok(new { token });
});

app.Run();
