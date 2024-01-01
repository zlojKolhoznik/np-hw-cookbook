using System.Text.Json.Serialization;

namespace CookBookApi.DTO;

public class RecipeDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("preparation")]
    public string Preparation { get; set; }
    
    [JsonPropertyName("ingredients")]
    public IEnumerable<string> Ingredients { get; set; }
}