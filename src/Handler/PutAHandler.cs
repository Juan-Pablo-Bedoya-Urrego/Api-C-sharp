namespace TodoApi.src.Handler;
using Microsoft.AspNetCore.Mvc;
using TodoApi.src.Config;
using TodoApi.src.Entities;

public class PutAHandler{
    private DbMemory _db;
    private Todo _todoUpdate;
    private int id;

    internal PutAHandler(DbMemory db, Todo todoUpdate, int id){
        this._db = db;
        this._todoUpdate = todoUpdate;
        this.id = id;
    }

    public async Task<IActionResult> Handler(){
        var todo = this._db.Todos.FirstOrDefault(
        item => item.Id == this.id
        );
        if (todo == null){
            throw new ArgumentException($"El pokemon con el id {this.id} no existe");
        }
        this._db.Entry(todo).CurrentValues.SetValues(this._todoUpdate);
        await this._db.SaveChangesAsync();
        return new OkResult();
    }
}