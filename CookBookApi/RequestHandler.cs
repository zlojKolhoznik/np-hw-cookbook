using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using CookBookApi.Data;
using CookBookApi.DTO;
using CookBookApi.Models;
using CookBookApi.Network;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Response = CookBookApi.Network.Response;

namespace CookBookApi;

public class RequestHandler : BackgroundService
{
    private readonly CookBookContext _context;
    private IPEndPoint? _remoteEndPoint;

    public RequestHandler(CookBookContext context)
    {
        _context = context;
    }
    
    ~RequestHandler()
    {
        _context.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Listen(stoppingToken);
        }
    }

    private async Task Listen(CancellationToken stoppingToken)
    {
        var listener = new UdpClient(Configuration.ListenPort);
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Waiting for broadcast . . .");
                var message = await ReceiveMessage(listener, stoppingToken);
                var request = JsonSerializer.Deserialize<Request>(message);
                if (request is null)
                {
                    throw new Exception("Serialization failed");
                }

                var response = request.Type switch
                {
                    RequestType.GetRecipes => await GetRecipesAsync(request, stoppingToken),
                    RequestType.GetImage => await GetImageAsync(request, stoppingToken),
                    RequestType.AddRecipe => await AddRecipeAsync(request, listener, _remoteEndPoint!, stoppingToken),
                    RequestType.DeleteRecipe => await DeleteRecipeAsync(request, stoppingToken),
                    _ => await Task.FromResult(new Response { Status = ResponseStatus.Error, ErrorMessage = "Invalid request type" })
                };

                await SendResponse(request, response, listener);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = new Response { Status = ResponseStatus.Error, ErrorMessage = e.Message };
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
            await listener.SendAsync(bytes, bytes.Length, _remoteEndPoint);
        }
        finally
        {
            listener.Close();
        }
    }

    private async Task SendResponse(Request request, Response response, UdpClient listener)
    {
        byte[] responseBytes;
        if (request.Type == RequestType.GetImage && response.Status == ResponseStatus.Success)
        {
            responseBytes = response.Image!;
        }
        else
        {
            responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
        }

        await listener.SendAsync(responseBytes, responseBytes.Length, _remoteEndPoint);
    }

    private async Task<string> ReceiveMessage(UdpClient listener, CancellationToken stoppingToken)
    {
        var result = await listener.ReceiveAsync(stoppingToken);
        _remoteEndPoint = result.RemoteEndPoint;
        var bytes = result.Buffer;
        var message = Encoding.UTF8.GetString(bytes);
        Console.WriteLine($"Received broadcast from {_remoteEndPoint} at {DateTime.Now}:");
        return message;
    }

    private async Task<Response> DeleteRecipeAsync(Request request, CancellationToken stoppingToken)
    {
        if (request.RecipeId is null)
        {
            throw new ArgumentException("RecipeId must not be null", nameof(request));
        }

        var recipe = await _context.Recipes.FindAsync([request.RecipeId], cancellationToken: stoppingToken);
        if (recipe is null)
        {
            throw new Exception($"Recipe with id {request.RecipeId} not found");
        }

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync(stoppingToken);
        return new Response
        {
            Status = ResponseStatus.Success
        };
    }

    private async Task<Response> AddRecipeAsync(Request request, UdpClient listener, IPEndPoint remoteEndPoint,
        CancellationToken stoppingToken)
    {
        if (request.RecipeName is null || request.RecipePreparation is null || request.Ingredients is null)
        {
            throw new ArgumentException("RecipeName, RecipePreparation and Ingredients must not be null",
                nameof(request));
        }

        var ingredients = await ValidateIngredients(request, stoppingToken);
        var recipe = new Recipe
        {
            Name = request.RecipeName,
            Preparation = request.RecipePreparation,
            Ingredients = ingredients
        };
        await _context.Recipes.AddAsync(recipe, stoppingToken);
        await _context.SaveChangesAsync(stoppingToken);
        await ReceiveImage(recipe, listener, remoteEndPoint, stoppingToken);
        return new Response
        {
            Status = ResponseStatus.Success
        };
    }

    private async Task ReceiveImage(Recipe recipe, UdpClient listener, IPEndPoint remoteEndPoint,
        CancellationToken stoppingToken)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Response { Status = ResponseStatus.ExcpectingImage }));
        await listener.SendAsync(bytes, bytes.Length, remoteEndPoint);
        var result = await listener.ReceiveAsync(stoppingToken);
        var image = result.Buffer;
        recipe.Image = image;
        await _context.SaveChangesAsync(stoppingToken);
    }

    private async Task<ICollection<Ingredient>> ValidateIngredients(Request request, CancellationToken stoppingToken)
    {
        var ingredients = new List<Ingredient>();
        foreach (var ingredient in request.Ingredients!)
        {
            if (await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredient, stoppingToken) is { } ingredientFromDb)
            {
                ingredients.Add(ingredientFromDb);
            }
            else
            {
                var newIngredient = new Ingredient { Name = ingredient };
                await _context.Ingredients.AddAsync(newIngredient, stoppingToken);
                await _context.SaveChangesAsync(stoppingToken);
                ingredients.Add(newIngredient);
            }
        }

        return ingredients;
    }

    private async Task<Response> GetImageAsync(Request request, CancellationToken stoppingToken)
    {
        if (request.RecipeId is null)
        {
            throw new ArgumentException("RecipeId must not be null", nameof(request));
        }

        var recipe = await _context.Recipes.FindAsync([request.RecipeId], cancellationToken: stoppingToken);
        if (recipe is null)
        {
            throw new Exception($"Recipe with id {request.RecipeId} not found");
        }

        return new Response
        {
            Status = ResponseStatus.Success,
            Image = recipe.Image
        };
    }

    private async Task<Response> GetRecipesAsync(Request request, CancellationToken stoppingToken)
    {
        if (request.Ingredients is null)
        {
            throw new ArgumentException("Ingredients must not be null", nameof(request));
        }

        var recipes = await _context.Recipes.Include(r => r.Ingredients)
            .Where(r => r.Ingredients.Any(i => request.Ingredients!.Contains(i.Name)))
            .ToListAsync(stoppingToken);
        var dtos = recipes.Select(r => new RecipeDto
        {
            Id = r.Id,
            Name = r.Name,
            Preparation = r.Preparation,
            Ingredients = r.Ingredients.Select(i => i.Name)
        });
        return new Response
        {
            Status = ResponseStatus.Success,
            Recipes = dtos
        };
    }
}