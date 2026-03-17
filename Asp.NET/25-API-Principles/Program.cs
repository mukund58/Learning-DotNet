using Microsoft.EntityFrameworkCore;
using _25_API_Principles.Models;
using _25_API_Principles.Dto;
using System.Data;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = Student.db");
});
builder.Services.AddControllers()
                .AddXmlSerializerFormatters();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var studentGroup = app.MapGroup("/api/students");

studentGroup.MapGet("", async (AppDbContext context) =>
{
    return await context.Students.ToListAsync();
});
studentGroup.MapGet("{id}", async (int id, AppDbContext context) =>
{
    var student = await context.Students.FindAsync(id);
    if (student is null) return Results.NotFound("Student Id Not Found");
    return Results.Ok(student);
});

studentGroup.MapGet("{id}/format", async (
    int id,
    AppDbContext context,
    HttpContext httpContext) =>
{
    var student = await context.Students.FindAsync(id);

    if (student is null)
        return Results.NotFound();

    var accept = httpContext.Request.Headers.Accept.ToString();

    if (accept.Contains("xml"))
    {
        var serializer = new XmlSerializer(typeof(Student));

        using var writer = new StringWriter();
        serializer.Serialize(writer, student);

        return Results.Content(
            writer.ToString(),
            "application/xml"
        );
    }

    if (accept.Contains("json") || string.IsNullOrWhiteSpace(accept))
    {
        return Results.Json(student);
    }

    return Results.StatusCode(406);
});

studentGroup.MapDelete("{id}", async (int id, AppDbContext context) =>
{
    var exists = await context.Students.FindAsync(id);
    if (exists is null) return Results.NotFound("Student Id Not Found");
    context.Students.Remove(exists);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

studentGroup.MapPost("", async (CreateStudentDto studentDto, AppDbContext context) =>
{
    var student = new Student
    {
        Name = studentDto.Name,
        Department = studentDto.Department
    };
    await context.AddAsync(student);
    await context.SaveChangesAsync();
    return Results.Created($"/api/students/{student.Id}", student);
});
studentGroup.MapPut("{id}", async (int id, Student student, AppDbContext context) =>
{
    if (id != student.Id)
        return Results.BadRequest("Route id and body id mismatch");

    var exists = await context.Students.FindAsync(id);
    if (exists is null) return Results.NotFound("Student Id Not Found");
    exists.Name = student.Name;
    exists.Department = student.Department;

    await context.SaveChangesAsync();
    return Results.NoContent();
});
studentGroup.MapPatch("{id}", async (int id, UpdateDepartmentDto departmentDto, AppDbContext context) =>
{
    if (string.IsNullOrWhiteSpace(departmentDto.Department))
        return Results.BadRequest("Department required");

    var exists = await context.Students.FindAsync(id);
    if (exists is null) return Results.NotFound("Student Id Not Found");
    exists.Department = departmentDto.Department;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

// app.UseHttpsRedirection();

app.Run();
