using Microsoft.EntityFrameworkCore;
using TodoApi.src.Config;
using TodoApi.src.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbMemory>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/todo/get", async (DbMemory db) =>
{
    return await db.Todos.ToListAsync();
});

app.MapGet("/todo/get/{id}", async (string id, DbMemory db) =>
{
    Todo? todo = await db.Todos.FindAsync(id);
    if (todo == null)
    {
        return Results.BadRequest($"El {id} no se encuentra");
    }
    return Results.Ok(todo);
});

app.MapGet("/todo/get/true", async (DbMemory db) =>
{
    int nose = db.Todos.Count();
    if (nose > 0)
    {
        foreach (var todo in db.Todos)
        {
            if (todo.isCompleted == true)
            {
                return Results.Ok(todo);
            }
        }
    }

    return Results.BadRequest($"No hay datos");
});

app.MapPost("/todo/post", async (Todo todo, DbMemory db) =>
{
    if (todo.nombre == "")
    {
        return Results.BadRequest("El campo nombre es vacio");
    }

    if (todo.isCompleted.GetType() != typeof(bool))
    {
        return Results.BadRequest("No es boleano");
    }
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/api/v1/Todo/{todo.Id}", todo);
});

app.MapDelete("/todo/delete/{id}", async (string id, DbMemory db) =>
{
    Todo? todo = await db.Todos.FindAsync(id);
    if (todo == null)
    {
        return Results.NotFound($"El todo con {id} no existe");
    }
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.Ok($"Todo con {id} eliminado correctamente");
});

app.MapPut("/todo/actualizar/{id}", async (string id, Todo todo_inpu, DbMemory db) =>
{
    Todo? todo = await db.Todos.FindAsync(id);
    if (todo == null)
    {
        return Results.NotFound($"El todo con {id} no existe");
    }
    todo.nombre = todo_inpu.nombre;
    todo.isCompleted = todo_inpu.isCompleted;
    await db.SaveChangesAsync();

    return Results.Ok($"Todo {id} actualizado");
});

app.Run();
