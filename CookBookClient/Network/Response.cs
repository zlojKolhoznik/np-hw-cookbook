using System.Text.Json.Serialization;
using CookBookClient.DTO;

namespace CookBookClient.Network;

public class Response
{
    [JsonPropertyName("status")]
    public ResponseStatus Status { get; set; }
    
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
    
    [JsonPropertyName("recipes")]
    public IEnumerable<RecipeDto>? Recipes { get; set; }
    
    [JsonPropertyName("image")]
    public byte[]? Image { get; set; }
}

public enum ResponseStatus
{
    Success,
    Error,
    ExcpectingImage
}