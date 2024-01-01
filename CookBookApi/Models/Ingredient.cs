using System.ComponentModel.DataAnnotations;

namespace CookBookApi.Models;

public class Ingredient
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public ICollection<Recipe> Recipes { get; set; }
}