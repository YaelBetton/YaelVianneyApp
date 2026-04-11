using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YaelApp.Models;
using YaelApp.Services;

namespace YaelApp.ViewModels
{
    public partial class VolleyScoreViewModel : ObservableObject
    {
        // --- Noms des équipes ---
        [ObservableProperty] private string teamAName = "Équipe A";
        [ObservableProperty] private string teamBName = "Équipe B";

        // --- Score du set en cours ---
        [ObservableProperty] private int scoreA;
        [ObservableProperty] private int scoreB;

        // --- Sets remportés ---
        [ObservableProperty] private int setsA;
        [ObservableProperty] private int setsB;

        // --- Set en cours (1 à 5) ---
        [ObservableProperty] private int currentSet = 1;

        // --- États de la page ---
        [ObservableProperty] private bool matchStarted;
        [ObservableProperty] private bool matchFinished;
        [ObservableProperty] private bool matchSaved;

        // --- Résultat final ---
        [ObservableProperty] private string winner = "";
        [ObservableProperty] private string setHistory = "";

        // --- Propriétés inversées pour la visibilité XAML (pas de CommunityToolkit.Maui ici) ---
        public bool IsSetupVisible => !MatchStarted;
        public bool IsLiveVisible => MatchStarted;
        public bool AreButtonsVisible => MatchStarted && !MatchFinished;
        public bool IsFinishedVisible => MatchFinished;
        public bool IsSaveButtonVisible => MatchFinished && !MatchSaved;
        public bool IsSavedLabelVisible => MatchSaved;
        public bool HasHistory => MatchHistory.Count > 0;
        public bool NoHistory => MatchHistory.Count == 0;

        // --- Historique des matchs joués ---
        public ObservableCollection<MatchModel> MatchHistory => VolleyMatchService.Instance.LocalMatches;

        private readonly List<string> _setResults = new();

        private int PointsToWin => CurrentSet == 5 ? 15 : 25;

        public VolleyScoreViewModel()
        {
            MatchHistory.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(HasHistory));
                OnPropertyChanged(nameof(NoHistory));
            };
        }

        partial void OnMatchStartedChanged(bool value)
        {
            OnPropertyChanged(nameof(IsSetupVisible));
            OnPropertyChanged(nameof(IsLiveVisible));
            OnPropertyChanged(nameof(AreButtonsVisible));
        }

        partial void OnMatchFinishedChanged(bool value)
        {
            OnPropertyChanged(nameof(IsFinishedVisible));
            OnPropertyChanged(nameof(AreButtonsVisible));
            OnPropertyChanged(nameof(IsSaveButtonVisible));
        }

        partial void OnMatchSavedChanged(bool value)
        {
            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsSavedLabelVisible));
        }

        // --- Démarrer le match ---
        [RelayCommand]
        void StartMatch()
        {
            MatchStarted = true;
            MatchFinished = false;
            MatchSaved = false;
        }

        // --- Ajouter / retirer des points ---
        [RelayCommand]
        void AddPointA()
        {
            if (MatchFinished) return;
            ScoreA++;
            CheckSetWin();
        }

        [RelayCommand]
        void AddPointB()
        {
            if (MatchFinished) return;
            ScoreB++;
            CheckSetWin();
        }

        [RelayCommand]
        void RemovePointA()
        {
            if (ScoreA > 0) ScoreA--;
        }

        [RelayCommand]
        void RemovePointB()
        {
            if (ScoreB > 0) ScoreB--;
        }

        // --- Vérifier si un set est gagné (règles volley) ---
        private void CheckSetWin()
        {
            int limit = PointsToWin;
            bool aWins = ScoreA >= limit && ScoreA - ScoreB >= 2;
            bool bWins = ScoreB >= limit && ScoreB - ScoreA >= 2;

            if (aWins) RegisterSetWin(isTeamA: true);
            else if (bWins) RegisterSetWin(isTeamA: false);
        }

        private void RegisterSetWin(bool isTeamA)
        {
            _setResults.Add($"Set {CurrentSet} : {ScoreA}-{ScoreB}");
            SetHistory = string.Join("   |   ", _setResults);

            if (isTeamA) SetsA++;
            else SetsB++;

            if (SetsA == 3 || SetsB == 3)
            {
                MatchFinished = true;
                Winner = SetsA == 3 ? TeamAName : TeamBName;
            }
            else
            {
                CurrentSet++;
                ScoreA = 0;
                ScoreB = 0;
            }
        }

        // --- Enregistrer le match dans Score ---
        [RelayCommand]
        void SaveMatch()
        {
            // Format "25/18 - 18/25 - 25/20" (cohérent avec MatchModel.TennisSetsFormatted)
            var setStrings = _setResults.Select(r =>
            {
                var scores = r.Split(": ")[1].Split("-");
                return $"{scores[0]}/{scores[1]}";
            });

            var match = new MatchModel
            {
                Id = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                NomMatch = $"{TeamAName} vs {TeamBName}",
                EquipeDomicile = TeamAName,
                EquipeExterieur = TeamBName,
                ScoreHomeRaw = SetsA.ToString(),
                ScoreAwayRaw = SetsB.ToString(),
                ResultatRaw = string.Join(" - ", setStrings),
                Sport = "Volleyball"
            };

            VolleyMatchService.Instance.AddMatch(match);
            MatchSaved = true;
        }

        // --- Réinitialiser pour un nouveau match ---
        [RelayCommand]
        void ResetMatch()
        {
            ScoreA = 0;
            ScoreB = 0;
            SetsA = 0;
            SetsB = 0;
            CurrentSet = 1;
            MatchStarted = false;
            MatchFinished = false;
            MatchSaved = false;
            Winner = "";
            SetHistory = "";
            _setResults.Clear();
        }
    }
}
