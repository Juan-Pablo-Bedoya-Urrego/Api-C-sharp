using Microsoft.EntityFrameworkCore;
using TodoApi.src.Config;
using TodoApi.src.Entities;
using TodoApi.src.Handler;
using TodoApi.src.validation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbMemory>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

/*Crear 1 pokemon*/
app.MapPost("/post/pokemon", async (HttpContext context, DbMemory db) =>
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
        ValidationTodo isValid = new ValidationTodo(todo);
        string textValidation = isValid.isValidMessage();
        if (!isValid.isValidBool())
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(textValidation);
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

/*Crear multiples pokemones*/
app.MapPost("/post/pokemones/multiples", async (HttpContext context, DbMemory db) =>
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
            if (todo == null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("JSON vacio");
                return;
            }
            ValidationTodo isValid = new ValidationTodo(todo);
            string textValidation = isValid.isValidMessage();
            if (!isValid.isValidBool())
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(textValidation);
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

/*Editar 1 pokemon*/
app.MapPut("/put/pokemon/{id:int}", async (int id, HttpContext context, DbMemory db) =>
{
    try
    {
        Todo? todoUpdate = await context.Request.ReadFromJsonAsync<Todo>();
        if (todoUpdate == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("JSON vacio");
            return;
        }
        ValidationTodo isValid = new ValidationTodo(todoUpdate);
        string textValidation = isValid.isValidMessage();
        if (!isValid.isValidBool())
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(textValidation);
            return;
        }
        PutAHandler put = new PutAHandler(db, todoUpdate, id);
        await put.Handler();
    }
    catch (Exception e)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(e.Message);
        return;
    }
});

/*Eliminar 1 pokemon*/
app.MapDelete("/delete/pokemon/{id:int}", async (int id, HttpContext context, DbMemory db) =>
{
    try
    {
        DeleteAHandler delete = new DeleteAHandler(db, id);
        await delete.Delete();
    }
    catch (Exception e)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(e.Message);
        return;
    }
});

/*Traer 1 pokemon*/
app.MapGet("/get/pokemon/uno/{id:int}", async (int id, HttpContext context) =>
{
    try
    {
        DbMemory db = context.RequestServices.GetRequiredService<DbMemory>();
        GetATodoHandler handle = new GetATodoHandler(db, id);
        var todos = handle.Handle();
        return Results.Ok(todos);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
});

/*Traer todos los de un mismo tipo*/
app.MapGet("/get/pokemon/tipo/{type}", async (string type, HttpContext context) =>
{
    try
    {
        DbMemory db = context.RequestServices.GetRequiredService<DbMemory>();
        GetAllTypeTodoHandler handle = new GetAllTypeTodoHandler(db, type);
        var todos = handle.Handle();
        return Results.Ok(todos);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
});

/*Propios de los programadores*/

/*1. Traer todos los pokemones*/
app.MapGet("/get/pokemones", async (HttpContext context) =>
{
    try
    {
        DbMemory db = context.RequestServices.GetRequiredService<DbMemory>();
        GetAllTodoHandler handle = new GetAllTodoHandler(db);
        var todos = handle.Handle();
        return Results.Ok(todos);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
});

/*2. traer todo los pokemos cuya suma de ataques sea igual o mayor el ingresado por el usuario*/
app.MapGet("/get/pokemones/ataque/{mayor:int}", async (int mayor, HttpContext context) =>
{
    try
    {
        DbMemory db = context.RequestServices.GetRequiredService<DbMemory>();
        GetAttackMajor major = new GetAttackMajor(db, mayor);
        var todos = major.handler();
        return Results.Ok(todos);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
});

app.Run();
