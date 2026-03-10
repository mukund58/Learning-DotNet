using _22_relationships.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source = Employee.db"));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/employes", async (AppDbContext db) =>
{
    var employees = await db.Employees.ToListAsync();
    return Results.Ok(employees);
});

app.MapGet("/deparment", async (AppDbContext db) =>
{
    var deparment = await db.Departments.ToListAsync();
    return Results.Ok(deparment);
});

app.UseHttpsRedirection();


app.Run();

