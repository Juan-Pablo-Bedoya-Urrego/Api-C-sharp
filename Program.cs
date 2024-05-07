using Microsoft.EntityFrameworkCore;
using TodoApi.src.Config;
using TodoApi.src.Entities;
using TodoApi.src.Handler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbMemory>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/todo/get/handler/{id:int}", (int id, DbMemory db) =>
{
    try
    {
        GetATodoHandler handle = new GetATodoHandler(db, id);
        var todos = handle.Handle();
        return Results.Ok(todos);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
});

app.MapGet("/todo/get/handler/{type}", (string type, DbMemory db) =>
{
    try
    {
        GetAllTypeTodoHandler handle = new GetAllTypeTodoHandler(db, type);
        var todos = handle.Handle();
        return Results.Ok(todos);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
});

app.MapGet("/todo/get/handler", (DbMemory db) =>
{
    try
    {
        GetAllTodoHandler handle = new GetAllTodoHandler(db);
        var todos = handle.Handle();
        return Results.Ok(todos);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
});

app.MapPost("/todo/post/multiple/handler", async (HttpContext context, DbMemory db) =>
{
    try
    {
        List<Todo>? todos = await context.Request.ReadFromJsonAsync<List<Todo>>();
        if (todos == null || todos.Count == 0)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("JSON vacio");
            return;
        }
        foreach (var todo in todos)
        {
            if (todo.Name == "")
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            foreach (var habilities in todo.SetHabilities)
            {
                if (habilities < 0 || habilities > 40)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("los valores de ataque deben ser entre 0 y 40");
                    return;
                }
            }
            if (todo.SetHabilities.Count > 4)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("los ataques deben ser maximo 4");
                return;
            }
            if (todo.SetHabilities.Count == 0)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("debe tener al menos 1 ataque");
                return;
            }
            if (todo.Defense > 30 || todo.Defense < 1)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("La defensa debe ser entre 1 y 30");
                return;
            }
        }
        CreateMultipleTodoHandler handler = new CreateMultipleTodoHandler(db);
        await handler.HandleAsync(todos);
    }
    catch (Exception e)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(e.Message);
        return;
    }
});

app.MapPost("/todo/post/handler", async (HttpContext context, DbMemory db) =>
{
    try
    {
        Todo? todo = await context.Request.ReadFromJsonAsync<Todo>();
        if (todo == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("JSON vacio");
            return;
        }
        if (todo.Name == "")
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        foreach (var habilities in todo.SetHabilities)
        {
            if (habilities < 0 || habilities > 40)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("los valores de ataque deben ser entre 0 y 40");
                return;
            }
        }
        if (todo.SetHabilities.Count > 4)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("los ataques deben ser maximo 4");
            return;
        }
        if (todo.SetHabilities.Count == 0)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("debe tener al menos 1 ataque");
            return;
        }
        if (todo.Defense > 30 || todo.Defense < 1)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("La defensa debe ser entre 1 y 30");
            return;
        }
        CreateTodoHandler handler = new CreateTodoHandler(db);
        await handler.HandleAsync(todo);
    }
    catch (Exception e)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(e.Message);
        return;
    }
});

app.Run();
