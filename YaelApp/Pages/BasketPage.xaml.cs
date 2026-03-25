using YaelApp.Services;

namespace YaelApp.Pages;

public partial class BasketPage : ContentPage
{
    private readonly SportsService _sportsService;

    public BasketPage(SportsService service)
    {
        InitializeComponent();
        _sportsService = service;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try 
        {
            // Appel du service pour le basket (id 4387)
            var matchs = await _sportsService.GetDerniersMatchsAsync("4387");
            
            MaListeDeMatchs.ItemsSource = matchs;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
