namespace TodoApi.src.Handler;
using Microsoft.AspNetCore.Mvc;
using TodoApi.src.Config;
using TodoApi.src.Entities;

public class GetAllTodoHandler
{
    private DbMemory _db;

    internal GetAllTodoHandler(DbMemory db){
        this._db = db;
    }

    public IEnumerable<Todo> Handle()
    {
        return this._db.Todos.ToList();
    }
}