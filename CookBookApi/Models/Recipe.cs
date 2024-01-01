using System.ComponentModel.DataAnnotations;

namespace CookBookApi.Models;

public class Recipe
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Preparation { get; set; }
    
    public byte[]? Image { get; set; }
    
    public ICollection<Ingredient> Ingredients { get; set; }
}