namespace YaelApp.Pages;

public partial class GifDuJourPage : ContentPage
{
    private const int GifDurationMilliseconds = 5000;
    private bool _canGoBack;
    private CancellationTokenSource? _delayCts;

    public GifDuJourPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _canGoBack = false;
        RetourAccueilButton.IsVisible = false;
        RetourAccueilButton.IsEnabled = false;
        InfoLabel.Text = "Lecture en cours...";

        _delayCts?.Cancel();
        _delayCts?.Dispose();
        _delayCts = new CancellationTokenSource();

        try
        {
            await Task.Delay(GifDurationMilliseconds, _delayCts.Token);
            _canGoBack = true;
            InfoLabel.Text = "Lecture terminee. Vous pouvez revenir a l'accueil.";
            RetourAccueilButton.IsVisible = true;
            RetourAccueilButton.IsEnabled = true;
        }
        catch (TaskCanceledException)
        {
            // The page has been closed before the delay ends.
        }
    }

    protected override void OnDisappearing()
    {
        _delayCts?.Cancel();
        _delayCts?.Dispose();
        _delayCts = null;

        base.OnDisappearing();
    }

    protected override bool OnBackButtonPressed()
    {
        return !_canGoBack || base.OnBackButtonPressed();
    }

    private async void OnRetourAccueilClicked(object sender, EventArgs e)
    {
        if (!_canGoBack)
        {
            return;
        }

        await Shell.Current.GoToAsync("..");
    }
}
