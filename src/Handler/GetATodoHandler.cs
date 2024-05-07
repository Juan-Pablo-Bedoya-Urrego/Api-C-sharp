namespace TodoApi.src.Handler;
using Microsoft.AspNetCore.Mvc;
using TodoApi.src.Config;
using TodoApi.src.Entities;

public class GetATodoHandler
{
    private DbMemory _db;
    private int id;

    internal GetATodoHandler(DbMemory db, int Id)
    {
        this._db = db;
        this.id = Id;
    }
    public Todo Handle()
    {
        var todo = this._db.Todos.FirstOrDefault(
        item => item.Id == this.id
        );
        if (todo == null){
            throw new ArgumentException($"El pokemon con el id {this.id} no existe");
        }
        return todo;
    }
}