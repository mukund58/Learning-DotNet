using Microsoft.EntityFrameworkCore;
using _25_API_Principles.Models;
using _25_API_Principles.Dto;
using System.Data;
using System.Xml.Serialization;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = Student.db");
});
builder.Services.AddControllers()
                .AddXmlSerializerFormatters();
builder.Services.AddScoped<IValidator<CreateStudentDto>, CreateStudentValidator>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var studentGroup = app.MapGroup("/api/students");
var v1Group = app.MapGroup("/api/v1/students");
var v2Group = app.MapGroup("/api/v2/students");

// version 1
v1Group.MapGet("/{id}", async (int id, AppDbContext context,HttpContext httpContext) =>
{
    var student = await context.Students.FindAsync(id);
    if (student is null) return Results.NotFound("Student Id Not Found");

    var v1StudentDto = new V1StudentDto
    {
        Id = student.Id,
        Name = student.Name,
        Department = student.Department
    };
    
    httpContext.Response.Headers.Append("Deprecation", "True");
    httpContext.Response.Headers.Append("Sunset", "Tue, 17 Mar 2026 18:00:00 GMT");
    httpContext.Response.Headers.Append("Link", "</api/v2/students/>; rel=\"successor-version\"");
    return Results.Ok(v1StudentDto);
});

// version 2
v2Group.MapGet("/{id}", async(int id, AppDbContext context) =>
{
    var student = await context.Students.FindAsync(id);
    if (student is null) return Results.NotFound("Student Id Not Found");

    var v2StudentDto = new V2StudentDto
    {
        Id = student.Id.ToString(),
        Name = student.Name,
        Department = student.Department
    };
    
    return Results.Ok(v2StudentDto);
});

// studentGroup.MapGet("", async (AppDbContext context) =>
// {
//     return await context.Students.ToListAsync();
// });
// studentGroup.MapGet("{id}", async (int id, AppDbContext context) =>
// {
//     var student = await context.Students.FindAsync(id);
//     if (student is null) return Results.NotFound("Student Id Not Found");
//     return Results.Ok(student);
// });

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

studentGroup.MapPost("", async (
    CreateStudentDto studentDto,
    IValidator<CreateStudentDto> validator,
    AppDbContext context) =>
{
    var result = await validator.ValidateAsync(studentDto);

    if (!result.IsValid)
    {
        var errors = result.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );

        return Results.ValidationProblem(errors);
    }

    var student = new Student
    {
        Name = studentDto.Name,
        Department = studentDto.Department
    };

    await context.Students.AddAsync(student);
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
