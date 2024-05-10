using TodoApi.src.Config;
using TodoApi.src.Entities;

namespace TodoApi.src.validation;

public class ValidationTodo{
    private Todo _todo;

    internal ValidationTodo(Todo todo){
        this._todo = todo;
    }

    public bool isValidBool(){
        if (string.IsNullOrWhiteSpace(this._todo.Name))
            return false;

        foreach (var habilities in this._todo.SetAttack)
        {
            if (habilities < 0 || habilities > 40)
                return false;
        }

        if (this._todo.SetAttack.Count > 4)
            return false;

        if (this._todo.SetAttack.Count == 0)
            return false;

        if (this._todo.Defense > 30 || this._todo.Defense < 1)
            return false;

        return true; 

    }

    public string isValidMessage(){
        if (string.IsNullOrWhiteSpace(this._todo.Name))
            return "El nombre de la tarea no puede estar vacío";

        foreach (var habilities in this._todo.SetAttack)
        {
            if (habilities < 0 || habilities > 40)
                return "Los valores de ataque deben ser entre 0 y 40";
        }

        if (this._todo.SetAttack.Count > 4)
            return "Los ataques deben ser máximo 4";

        if (this._todo.SetAttack.Count == 0)
            return "Debe tener al menos 1 ataque";

        if (this._todo.Defense > 30 || this._todo.Defense < 1)
            return "La defensa debe ser entre 1 y 30";
        return ""; 
    }
}