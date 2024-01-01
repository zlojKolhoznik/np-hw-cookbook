using System.Text.Json.Serialization;

namespace CookBookApi.Network;

public class Request
{
    [JsonPropertyName("type")]
    public RequestType Type { get; set; }
    
    [JsonPropertyName("ingredients")]
    public IEnumerable<string>? Ingredients { get; set; }
    
    [JsonPropertyName("recipeId")] 
    public int? RecipeId { get; set; }
    
    [JsonPropertyName("recipeName")]
    public string? RecipeName { get; set; }
    
    [JsonPropertyName("recipePreparation")]
    public string? RecipePreparation { get; set; }
}

public enum RequestType
{
    GetRecipes,
    GetImage,
    AddRecipe,
    DeleteRecipe
}