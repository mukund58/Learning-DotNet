var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IMessageService, MessageService>();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>(); // BEFORE MapControllers

app.MapControllers();

app.Run();
