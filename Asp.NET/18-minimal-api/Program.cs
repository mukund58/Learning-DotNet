var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); // Required for Minimal APIs


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


var items = new List<string> {
    "apple",
    "banana"
};
app.MapGet("/todo", () => {
    return Results.Ok(items);
});

app.MapGet("/todo/{id:int}", (int id) => {
    if (id < 0) {
        return Results.BadRequest("Invalid ID");
    } else if (items.Count <= id) {
        return Results.NotFound("Item Not Found");
    }
    return Results.Ok(items[id]);
});

app.MapPut("/todo/{id:int}",(int id,string data) => {
    if (data == "" || data is null || data == " " || data.Contains("\t"))
    {
        return Results.BadRequest();
    }
    if (id < 0) {
        return Results.BadRequest("Invalid ID");
    } else if (items.Count <= id) {
        return Results.NotFound("Item Not Found");
    }
    items[id] = data;
    return Results.Created($"/todo/{items[id]}",data);

});
app.MapPost("/todo", (string data) => {
    if (data == "" || data is null || data == " " || data.Contains("\t"))
    {
        return Results.BadRequest();
    }
    data = data.ToLower().Trim();
    bool found = items.Contains(data);
    if(found){
        return Results.BadRequest();
    }
    items.Add(data);
    return Results.Created($"/todo/{items.IndexOf(data+1)}",data);
});

app.MapDelete("/todo", (string data) => {
    if (data == "" || data is null || data == " " || data.Contains("\t"))
    {
        return Results.BadRequest();
    }
    data = data.ToLower().Trim();
    bool found = items.Contains(data);
    if(!found){
        return Results.NotFound("Item Not Found");
    }
    items.Remove(data);
    return Results.NoContent();
});

app.Run();

