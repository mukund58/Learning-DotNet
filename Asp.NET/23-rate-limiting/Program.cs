using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder =  QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});
builder.Services.AddRateLimiter(options =>
{
    options.AddTokenBucketLimiter("token", opt =>
    {
        opt.TokenLimit = 10;
        opt.TokensPerPeriod = 2;
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(1);
    });
});

var app = builder.Build();
app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGet("/", () => "Hello")
   .RequireRateLimiting("fixed");

app.MapGet("/bucket", () => "Hello")
   .RequireRateLimiting("token");

app.Run();

