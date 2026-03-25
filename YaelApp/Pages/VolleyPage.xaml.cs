using YaelApp.Services;

namespace YaelApp.Pages;

public partial class VolleyPage : ContentPage
{
    private readonly SportsService _sportsService;

    public VolleyPage(SportsService service)
    {
        InitializeComponent();
        _sportsService = service;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Appel du service pour le volley (id 5083)
        var matchs = await _sportsService.GetDerniersMatchsAsync("5083");
        
        MaListeDeMatchs.ItemsSource = matchs;
    }
}

