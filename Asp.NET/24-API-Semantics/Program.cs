using _24_API_Semantics.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite("Data Source=Todo.db"));

var app = builder.Build();

var idempotencyStore = new Dictionary<string, Todo>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapGet("/todos", async (TodoDbContext db) =>
{
    return await db.Todos.ToListAsync();
});

// Just For Learning key stored in memory
// app.MapPost("/todos", async (
//     HttpContext context,
//     Todo todo,
//     TodoDbContext db) =>
// {
//     var key = context.Request.Headers["Idempotency-Key"].ToString();

//     if (string.IsNullOrWhiteSpace(key))
//         return Results.BadRequest("Missing Idempotency-Key");

//     if (idempotencyStore.ContainsKey(key))
//         return Results.Ok(idempotencyStore[key]);

//     await db.Todos.AddAsync(todo);
//     await db.SaveChangesAsync();

//     idempotencyStore[key] = todo;

//     return Results.Created($"/todos/{todo.Id}", todo);
// });

// Production Method key stored in class IdempotencyStore
// Client sends:
// Idempotency-Key: abc123

// Server:
// 1. Check key exists?
// 2. If exists → return stored response
// 3. If not exists:
//    - hash request
//    - save key + hash + response
//    - execute business logic

app.MapPost("/todos", async (
    HttpContext context,
    Todo todo,
    TodoDbContext db) =>
{
    var key = context.Request.Headers["Idempotency-Key"].ToString();

    if (string.IsNullOrWhiteSpace(key))
        return Results.BadRequest("Missing Idempotency-Key");

    // 1. Check key exists?
    var body = JsonSerializer.Serialize(todo);

    // hash request
    var hash = Convert.ToHexString(
        SHA256.HashData(Encoding.UTF8.GetBytes(body))
    );

    var existing = await db.IdempotencyRecords
        .FirstOrDefaultAsync(x => x.Key == key);

    // 2. If exists → return stored response
    if (existing is not null)
    {
        // key is same but hash is different (Violation of Data integrity)
        if (existing.RequestHash != hash)
            return Results.Conflict("Same key with different request");

        return Results.Content(
            existing.ResponseBody,
            "application/json",
            Encoding.UTF8,
            existing.StatusCode
        );
    }

    // 3. If not exists:
    // - execute business logic
    await db.Todos.AddAsync(todo);
    await db.SaveChangesAsync();

    var responseBody = JsonSerializer.Serialize(todo);

    // - save key + hash + response + status code
    await db.IdempotencyRecords.AddAsync(new IdempotencyRecord
    {
        Key = key,
        RequestHash = hash,
        ResponseBody = responseBody,
        StatusCode = 201
    });

    await db.SaveChangesAsync();
    return Results.Created($"/todos/{todo.Id}", todo);
});
app.MapPost("/webhooks/polar", async (HttpRequest request) =>
{
    var payload = await new StreamReader(request.Body).ReadToEndAsync();

    Console.WriteLine(payload);

    // update order status
    return Results.Ok();
});

app.MapPost("/create-checkout", async (IConfiguration config) =>
{
    var token = config["Polar:ApiToken"] ?? Environment.GetEnvironmentVariable("POLAR_ACCESS_TOKEN");
    var successUrl = config["Polar:SuccessUrl"] ?? Environment.GetEnvironmentVariable("POLAR_SUCCESS_URL");
    var returnUrl = config["Polar:ReturnUrl"] ?? Environment.GetEnvironmentVariable("POLAR_RETURN_URL");

    if (string.IsNullOrWhiteSpace(token))
        return Results.BadRequest(new { error = "Missing Polar API token", detail = "Set POLAR_ACCESS_TOKEN or Polar:ApiToken." });

    if (string.IsNullOrWhiteSpace(successUrl) || string.IsNullOrWhiteSpace(returnUrl))
        return Results.BadRequest(new { error = "Missing redirect urls", detail = "Set POLAR_SUCCESS_URL and POLAR_RETURN_URL or Polar:SuccessUrl/ReturnUrl." });

    using var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate });
    client.Timeout = TimeSpan.FromSeconds(10);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var body = new
    {
        products = new[] { "id" },
        success_url = successUrl,
        return_url = returnUrl
    };

    var response = await client.PostAsJsonAsync("https://sandbox-api.polar.sh/v1/checkouts", body);
    var responseText = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        return Results.Json(new { error = response.StatusCode.ToString(), detail = responseText }, statusCode: (int)response.StatusCode);
    }

    var json = JsonSerializer.Deserialize<JsonElement>(responseText);
    return Results.Ok(json);
});
app.Run();
