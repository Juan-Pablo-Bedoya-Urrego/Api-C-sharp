namespace TodoApi.src.Entities;
using System.ComponentModel.DataAnnotations;

public class Todo
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    public string Type { get; set; }

    public List<int> SetAttack { get; set; } = new List<int>();

    [Required(ErrorMessage = "Defense is required.")]
    public float Defense { get; set; }
}
