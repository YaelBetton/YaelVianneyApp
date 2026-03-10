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

    private void OnPreviousClicked(object? sender, EventArgs e)
    {
        if (NewsCarousel.ItemsSource is not System.Collections.IList items || items.Count == 0)
        {
            return;
        }

        var previous = NewsCarousel.Position - 1;
        NewsCarousel.Position = previous < 0 ? items.Count - 1 : previous;
    }

    private void OnNextClicked(object? sender, EventArgs e)
    {
        if (NewsCarousel.ItemsSource is not System.Collections.IList items || items.Count == 0)
        {
            return;
        }

        var next = NewsCarousel.Position + 1;
        NewsCarousel.Position = next >= items.Count ? 0 : next;
    }
}
