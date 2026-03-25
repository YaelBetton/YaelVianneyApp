using YaelApp.Services;

namespace YaelApp.Pages;

public partial class TennisPage : ContentPage
{
    private readonly SportsService _sportsService;

    public TennisPage(SportsService service)
    {
        InitializeComponent();
        _sportsService = service;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Appel du service pour le tennis (id 4464)
        var matchs = await _sportsService.GetDerniersMatchsAsync("4464");
        
        // On lie les données à une CollectionView dans le XAML
        MaListeDeMatchs.ItemsSource = matchs;
    }
}

