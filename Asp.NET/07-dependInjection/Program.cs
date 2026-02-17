var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IMessageService, MessageService>();

var app = builder.Build();

app.MapControllers();
app.Run();
