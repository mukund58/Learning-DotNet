using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthenticationDemo
{
    public class Program2
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer(); // Required for Minimal APIs
            builder.Services.AddSwaggerGen();

            var key = "super_secret_key_12345123456123456";

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    });

            // builder.Services.AddAuthorization();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy =>
                    policy.RequireRole("User", "Admin"));

                options.AddPolicy("Admin", policy =>
                    policy.RequireRole("Admin"));
            });
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();
''
            // ---------------------
            // SIGNUP / LOGIN (demo)
            // ---------------------.

            var users = new List<User>(); // In-memory store

            // Signup with role
            app.MapPost("/signup1", (User user) =>
            {
                if (users.Any(u => u.Username == user.Username))
                    return Results.BadRequest("User already exists");

                users.Add(user);

                var token = GenerateToken(user.Username, user.Role, key);
                return Results.Ok(new { token });
            });


            app.MapGet("/public", () => "Anyone can access!");
            app.MapGet("/", () => "hello world");

            app.MapGet("/general", () => "Hello General User!")
               .RequireAuthorization(new[] { "User" });


            app.MapGet("/admin1", () => "Hello Admin!")
               .RequireAuthorization(new[] { "Admin" });

            app.Run();


            string GenerateToken(string username, string role, string key)
            {
                var claims = new[]
                {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, role)
                 };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

             // Role = "Admin" or "User"
        }
        record User(string Username, string Password, string Role);
    }
}
