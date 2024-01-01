using System.Windows;
using System.Windows.Controls;
using CookBookClient.Network;

namespace CookBookClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void AddIngredientButtonClick(object sender, RoutedEventArgs e)
    {
        var textBox = new TextBox
        {
            Margin = new Thickness(20),
            Padding = new Thickness(5),
            FontSize = 20,
            Width = 300
        };
        IngredientsInputs.Children.Add(textBox);
        if (IngredientsInputs.Children.Count >= 5)
        {
            (sender as Button)!.IsEnabled = false;
        }
    }

    private async void SearchForRecipesButtonClick(object sender, RoutedEventArgs e)
    {
        var ingredients = IngredientsInputs.Children.OfType<TextBox>().Select(x => x.Text.ToLower())
            .Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
        var request = new Request
        {
            Type = RequestType.GetRecipes,
            Ingredients = ingredients
        };
        var connectionManager = new UdpConnectionManager();
        var response = await connectionManager.SendRequestAsync(request);
        if (response.Status == ResponseStatus.Error)
        {
            MessageBox.Show(response.ErrorMessage);
            return;
        }
        
        if (response.Recipes is null || !response.Recipes.Any())
        {
            MessageBox.Show("No recipes found");
            return;
        }

        var recipes = response.Recipes;
        var recipesWindow = new RecipesWindow(recipes);
        recipesWindow.Show();
        Application.Current.MainWindow = recipesWindow;
        Close();
    }

    private void AddRecipeButtonClick(object sender, RoutedEventArgs e)
    {
        var addRecipeWindow = new AddRecipeWindow();
        addRecipeWindow.ShowDialog();
        MessageBox.Show(addRecipeWindow.DialogResult == true ? "Recipe added successfully" : "Recipe was not added");
    }
}