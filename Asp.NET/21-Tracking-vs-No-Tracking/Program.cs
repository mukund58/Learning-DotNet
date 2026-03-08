using System;
using Microsoft.EntityFrameworkCore;
using _21_Tracking_vs_No_Tracking.Models;
using Microsoft.EntityFrameworkCore.Query;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = Empployee.db");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async (AppDbContext db) =>
{
    var Emp = await db.Employees
        .AsNoTracking()
        .ToListAsync();
    return Emp;

});

app.MapPatch("/getEmpTrack/{id}", async (int id, AppDbContext db) =>
{
    var Emp = await db.Employees.FirstOrDefaultAsync(e => e.Id == id);

    if (Emp is null) return Results.NotFound("Employee is not Found");
    Emp.Name = "Nix";
    await db.SaveChangesAsync();
    return Emp;

});

app.MapPatch("/getEmpNoTrack/{id}", async (int id, AppDbContext db) =>
{
    var Emp = await db.Employees
        .AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id);

    if (Emp is null) return Results.NotFound("Employee is not Found");
    Emp.Name = "NixNoTrack";
    await db.SaveChangesAsync();
    return Emp;

});

app.MapPatch("/getEmpTrackDetach/{id}", async (int id, AppDbContext db) =>
{


    var Emp = await db.Employees
        .AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id);

    if (Emp is null) return Results.NotFound("Employee is not Found");
    Emp.Name = "NixDetach";
    // db.Employees.Update(Emp);
    db.Attach(Emp);
    db.Entry(Emp).State = EntityState.Modified;

    await db.SaveChangesAsync();
    return Emp;

});

app.MapPost("/signup", async (Employee signup, AppDbContext db) =>
{

    var Emp = new Employee
    {
        Id = signup.Id,
        Name = signup.Name,
        Department = signup.Department
    };

    await db.Employees.AddAsync(Emp);
    await db.SaveChangesAsync();

    return Results.Created();
});

app.UseHttpsRedirection();


app.Run();
