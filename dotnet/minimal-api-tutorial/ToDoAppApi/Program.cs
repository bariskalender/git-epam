using ToDoAppApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var todos = new List<TodoItem>();
var idCounter = 1;

app.MapGet("/", () => "API çalışıyor");

app.MapGet("/todos", () => todos);

app.MapGet("/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(x => x.Id == id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/todos", (TodoItem item) =>
{
    item.Id = idCounter++;
    todos.Add(item);
    return Results.Created($"/todos/{item.Id}", item);
});

app.MapPut("/todos/{id}", (int id, TodoItem updated) =>
{
    var todo = todos.FirstOrDefault(x => x.Id == id);
    if (todo is null) return Results.NotFound();

    todo.Title = updated.Title;
    todo.IsCompleted = updated.IsCompleted;

    return Results.Ok(todo);
});

app.MapDelete("/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(x => x.Id == id);
    if (todo is null) return Results.NotFound();

    todos.Remove(todo);
    return Results.NoContent();
});

app.Run();
