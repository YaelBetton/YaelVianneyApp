using System.Collections.ObjectModel;
using YaelApp.Models;
using YaelApp.Services;

namespace YaelApp.Pages;
public partial class TennisPage : ContentPage
{
    private const int MaxSets = 5;
    private int currentSetCount = 0;
    private readonly List<(Entry, Entry)> setEntries = new();

    public TennisPage()
    {
        InitializeComponent();
        InitializeSets();
    }

    private void InitializeSets()
    {
        SetsStack.Children.Clear();
        setEntries.Clear();
        currentSetCount = 0;
        AddSet();
    }

    private void AddSet()
    {
        if (currentSetCount >= MaxSets) return;
        var setNum = currentSetCount + 1;
        var layout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 10 };
        var label = new Label { Text = $"Set {setNum}", WidthRequest = 60, VerticalTextAlignment = TextAlignment.Center };
        var entry1 = new Entry { Placeholder = "Joueur 1", WidthRequest = 60, Keyboard = Keyboard.Numeric };
        var entry2 = new Entry { Placeholder = "Joueur 2", WidthRequest = 60, Keyboard = Keyboard.Numeric };
        layout.Children.Add(label);
        layout.Children.Add(entry1);
        layout.Children.Add(new Label { Text = "/", VerticalTextAlignment = TextAlignment.Center });
        layout.Children.Add(entry2);
        SetsStack.Children.Add(layout);
        setEntries.Add((entry1, entry2));
        currentSetCount++;
        AddSetButton.IsEnabled = currentSetCount < MaxSets;
    }

    private void OnAddSetClicked(object sender, EventArgs e)
    {
        AddSet();
    }

    private async void OnSaveMatchClicked(object sender, EventArgs e)
    {
        string joueur1 = Player1Entry.Text;
        string joueur2 = Player2Entry.Text;
        DateTime dateMatch = MatchDatePicker.Date ?? DateTime.Today;
        if (string.IsNullOrWhiteSpace(joueur1) || string.IsNullOrWhiteSpace(joueur2))
        {
            await DisplayAlertAsync("Erreur", "Veuillez remplir les noms des joueurs.", "OK");
            return;
        }
        var sets = new List<string>();
        foreach (var (entry1, entry2) in setEntries)
        {
            var s1 = entry1.Text?.Trim();
            var s2 = entry2.Text?.Trim();
            if (string.IsNullOrWhiteSpace(s1) || string.IsNullOrWhiteSpace(s2))
            {
                await DisplayAlertAsync("Erreur", "Veuillez remplir tous les scores de sets.", "OK");
                return;
            }
            sets.Add($"{s1}/{s2}");
        }
        string scoreMatch = string.Join(" - ", sets);
        // Ajout du match dans le service partagé
        var match = new MatchModel
        {
            Id = Guid.NewGuid().ToString(),
            NomMatch = $"{joueur1} vs {joueur2}",
            EquipeDomicile = joueur1,
            EquipeExterieur = joueur2,
            ResultatRaw = scoreMatch,
            Sport = "Tennis",
            RawImageUrl = "tennis_image.jpg"
        };
        TennisMatchService.Instance.AddMatch(match);
        await DisplayAlertAsync("Succès", $"Match enregistré : {joueur1} vs {joueur2} le {dateMatch:dd/MM/yyyy} - Score : {scoreMatch}", "OK");
        Player1Entry.Text = string.Empty;
        Player2Entry.Text = string.Empty;
        MatchDatePicker.Date = DateTime.Today;
        InitializeSets();
    }
}
