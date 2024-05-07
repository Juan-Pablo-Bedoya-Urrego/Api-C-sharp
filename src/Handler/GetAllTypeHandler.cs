namespace TodoApi.src.Handler;
using Microsoft.AspNetCore.Mvc;
using TodoApi.src.Config;
using TodoApi.src.Entities;

public class GetAllTypeTodoHandler
{
    private DbMemory _db;
    private string Type;

    internal GetAllTypeTodoHandler(DbMemory db, string Type)
    {
        this._db = db;
        this.Type = Type;
    }
    public IEnumerable<Todo> Handle()
    {
        var todos = this._db.Todos.Where(item => item.Type == this.Type).ToList();
        return todos;
    }
}