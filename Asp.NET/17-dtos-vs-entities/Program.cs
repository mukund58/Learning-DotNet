using Microsoft.EntityFrameworkCore;
using 17-dtos-vs-entities.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
