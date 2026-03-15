using _22_relationships.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source = Employee.db"));
// builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
// {
//     options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
// });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/", () => "Api Is Working!!");
var EmployeeGroup = app.MapGroup("/api/employee");
var DepartmentGroup = app.MapGroup("/api/department");




EmployeeGroup.MapPost("/", async (Employee employee, AppDbContext db) =>
{
    await db.Employees.AddAsync(employee);
    await db.SaveChangesAsync();
    return Results.Created();
});


EmployeeGroup.MapGet("/", async (AppDbContext db) =>
{
    var employees = await db.Employees.ToListAsync();
    return Results.Ok(employees);
});

// EmployeeGroup.MapGet("/department", async (AppDbContext db) =>
// {
//     var employees = await db.Employees.Include(d => d.Department).ToListAsync();
//     return Results.Ok(employees);
// });
EmployeeGroup.MapGet("/department", async (AppDbContext db) =>
{
    return await db.Employees
        .Select(e => new
        {
            e.Id,
            e.Name,
            DepartmentName = e.Department.DepartmentName // Only pull what you need
        })
        .ToListAsync();
});

DepartmentGroup.MapGet("/", async (AppDbContext db) =>
{
    var departments = await db.Departments.ToListAsync();
    return Results.Ok(departments);
});

DepartmentGroup.MapGet("/employee", async (AppDbContext db) =>
{
    return await db.Departments.Include(e => e.Employees).Select(e => new
    {
        e.DepartmentId,
        e.DepartmentName
    })
       .ToListAsync();
});
DepartmentGroup.MapGet("/employeeAdress", async (AppDbContext db) =>
{
    return await db.Departments
        .Select(d => new
        {
            d.DepartmentName,
            EmployeeName = d.Employees.Select(e => new {
                e.Name,
                Address = e.EmployeeAddress != null ? e.EmployeeAddress.Address : "Address Not Found"
            }),
        }).ToListAsync();

});

DepartmentGroup.MapPost("/", async (Department department, AppDbContext db) =>
{
    // Clear nested employees to avoid deserialization/tracking issues
    // Employees should be associated via their DepartmentId property instead
    await db.Departments.AddAsync(department);
    await db.SaveChangesAsync();
    return Results.Created();
});

app.UseHttpsRedirection();


app.Run();
