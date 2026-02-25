using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// var key = "super_key_123456789";
var key = "this_is_a_super_secret_key_123456";


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var items = new List<string>();

app.MapGet("/", () => "Hello World!");

app.MapGet("/secure", () => "Authorized!")
   .RequireAuthorization();
   
   app.MapPost("/login", (string username,string password) =>
   {
       // string username = "admin";
       // string password = "admin";
       if (username != "admin" || password != "admin")
           return Results.Unauthorized();

        var token = new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken(
                claims: new[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256)
        ));
        // var token = new JwtSecurityTokenHandler()
        //     .WriteToken(new JwtSecurityToken());

       return Results.Ok(new { token });
   });
app.MapGet("/todo", () => Results.Ok(items));

app.MapGet("/todo/{id:int}", (int id) =>
{
    if (id < 0)
        return Results.BadRequest("Invalid ID");

    if (id >= items.Count)
        return Results.NotFound("Item Not Found");

    return Results.Ok(items[id]);
});

app.MapPost("/todo", (string data) =>
{
    if (string.IsNullOrWhiteSpace(data))
        return Results.BadRequest("Invalid item");

    data = data.ToLower().Trim();

    if (items.Contains(data))
        return Results.BadRequest("Item already exists");

    items.Add(data);
    return Results.Created($"/todo/{items.Count - 1}", data);
});

app.MapPut("/todo/{id:int}", (int id, string data) =>
{
    if (string.IsNullOrWhiteSpace(data))
        return Results.BadRequest("Invalid item");

    if (id < 0 || id >= items.Count)
        return Results.NotFound("Item Not Found");

    items[id] = data.ToLower().Trim();
    return Results.NoContent();
});

app.MapDelete("/todo/{id:int}", (int id) =>
{
    if (id < 0 || id >= items.Count)
        return Results.NotFound("Item Not Found");

    items.RemoveAt(id);
    return Results.NoContent();
});

app.Run();