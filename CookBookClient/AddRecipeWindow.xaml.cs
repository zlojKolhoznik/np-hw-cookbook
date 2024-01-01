using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CookBookClient.Network;
using Microsoft.Win32;

namespace CookBookClient;

public partial class AddRecipeWindow : Window
{
    public AddRecipeWindow()
    {
        InitializeComponent();
    }

    public string? ImageFilePath { get; set; }

    private void AddIngredientInput(object sender, RoutedEventArgs e)
    {
        var textBox = new TextBox
        {
            Margin = new Thickness(15),
            Padding = new Thickness(5),
            FontSize = 20
        };
        IngredientsInputs.Children.Add(textBox);
    }

    private void SelectImage(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            var image = new BitmapImage(new Uri(openFileDialog.FileName));
            if (image is not { Width: <= 256, Height: <= 256 })
            {
                MessageBox.Show("Image must be 256x256 or smaller");
                return;
            }

            ImageFilePath = openFileDialog.FileName;
            Image.Source = image;
        }
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private async void AddRecipe(object sender, RoutedEventArgs e)
    {
        var ingredients = IngredientsInputs.Children.OfType<TextBox>().Select(x => x.Text.ToLower())
            .Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
        if (ingredients.Count < 2)
        {
            MessageBox.Show("Recipe must have at least 2 ingredients");
            return;
        }

        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            MessageBox.Show("Recipe must have a name");
            return;
        }

        if (string.IsNullOrWhiteSpace(PreparationMethodTextBox.Text))
        {
            MessageBox.Show("Recipe must have a preparation method");
            return;
        }

        if (string.IsNullOrWhiteSpace(ImageFilePath))
        {
            MessageBox.Show("Recipe must have an image");
            return;
        }

        var connectionManager = new UdpConnectionManager();
        var request = new Request
        {
            Type = RequestType.AddRecipe,
            RecipeName = NameTextBox.Text,
            Ingredients = ingredients,
            RecipePreparation = PreparationMethodTextBox.Text
        };
        var response = await connectionManager.SendRequestAsync(request);
        if (response.Status != ResponseStatus.ExcpectingImage)
        {
            MessageBox.Show(response.ErrorMessage ?? "Unknown error");
            return;
        }

        var bytes = await File.ReadAllBytesAsync(ImageFilePath);
        await connectionManager.SendImageAsync(bytes);
        MessageBox.Show("Recipe added successfully");
        DialogResult = true;
    }
}