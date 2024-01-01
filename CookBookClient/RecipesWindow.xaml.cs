using System.Windows;
using System.Windows.Controls;
using CookBookClient.DTO;

namespace CookBookClient;

public partial class RecipesWindow : Window
{
    private IEnumerable<RecipeDto> _recipes;
    
    public RecipesWindow(IEnumerable<RecipeDto> recipes)
    {
        _recipes = recipes;
        InitializeComponent();
        foreach (var recipe in _recipes)
        {
            var recipeButton = new Button
            {
                Content = recipe.Name,
                Margin = new Thickness(10),
                Padding = new Thickness(5),
                FontSize = 20
            };
            recipeButton.Click += (sender, args) =>
            {
                var recipeWindow = new RecipeWindow(recipe);
                recipeWindow.Show();
                Application.Current.MainWindow = recipeWindow;
                Close();
            };
            MainPanel.Children.Add(recipeButton);
        }
    }
}