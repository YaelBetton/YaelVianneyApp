using YaelApp.ViewModels;

namespace YaelApp.Pages;

public partial class ScorePage : ContentPage
{
    private readonly ScoreViewModel _viewModel;

    public ScorePage(ScoreViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await _viewModel.LoadScoresAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur dans ScorePage: {ex.Message}");
        }
    }
}
