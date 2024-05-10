namespace TodoApi.src.Handler;
using Microsoft.AspNetCore.Mvc;
using TodoApi.src.Config;
using TodoApi.src.Entities;

public class GetAttackMajor{
    private DbMemory _db;
    private int _major;

    internal GetAttackMajor(DbMemory db,int major){
        this._db = db;
        this._major = major;
    }

    public IEnumerable<Todo> handler(){
        var todos = this._db.Todos.Where(item => item.SetAttack.Sum() >= this._major);
        if(todos == null){
            throw new ArgumentException($"No hay pokemones que superen {this._major} en la suma de sus ataques");
        }
        return todos; 
    }
}