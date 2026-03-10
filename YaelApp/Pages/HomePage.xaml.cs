namespace YaelApp.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        GifDuJourButton.Clicked += OnGifDuJourClicked;
    }

    private async void OnGifDuJourClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("GifDuJourPage");
    }
}
