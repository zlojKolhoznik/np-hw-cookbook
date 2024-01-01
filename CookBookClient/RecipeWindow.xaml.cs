using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using CookBookClient.DTO;
using CookBookClient.Network;

namespace CookBookClient;

public partial class RecipeWindow : Window
{
    private readonly RecipeDto _recipe;

    public RecipeWindow(RecipeDto recipe)
    {
        _recipe = recipe;
        InitializeComponent();
        NameTextBlock.Text = _recipe.Name;
        PreparationTextBlock.Text = _recipe.Preparation;
        foreach (var ingredient in _recipe.Ingredients)
        {
            IngredientsTextBlock.Text += $"- {ingredient}\n";
        }

        InitializeImage();
    }
    
    private async void InitializeImage()
    {
        var connectionManager = new UdpConnectionManager();
        var request = new Request
        {
            Type = RequestType.GetImage,
            RecipeId = _recipe.Id
        };
        var response = await connectionManager.SendRequestAsync(request);
        if (response.Status == ResponseStatus.Error)
        {
            MessageBox.Show(response.ErrorMessage);
            Close();
        }

        var image = response.Image;
        if (image is null)
        {
            MessageBox.Show("No image found");
            Close();
        }

        var imageSource = new BitmapImage();
        imageSource.BeginInit();
        imageSource.StreamSource = new MemoryStream(image!);
        imageSource.EndInit();
        Image.Source = imageSource;
    }

    private void BackToMenu(object sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        Application.Current.MainWindow = mainWindow;
        Close();
    }
}