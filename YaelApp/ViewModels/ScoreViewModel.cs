using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using YaelApp.Models;
using YaelApp.Services;

namespace YaelApp.ViewModels
{
    public partial class ScoreViewModel : ObservableObject
    {
        private readonly SportsService _sportsService;

        [ObservableProperty]
        private ObservableCollection<MatchModel> matches;

        [ObservableProperty]
        private ObservableCollection<MatchModel> tennisMatches;

        [ObservableProperty]
        private ObservableCollection<MatchModel> volleyMatches;

        [ObservableProperty]
        private bool isBusy;

        public ScoreViewModel(SportsService sportsService)
        {
            _sportsService = sportsService;
            Matches = new ObservableCollection<MatchModel>();
            TennisMatches = new ObservableCollection<MatchModel>();
            VolleyMatches = new ObservableCollection<MatchModel>();
        }

        [RelayCommand]
        public async Task LoadScoresAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var basketResult = await _sportsService.GetDerniersMatchsAsync("4387");
                var tennisResult = await _sportsService.GetDerniersMatchsAsync("4464");
                var volleyResult = await _sportsService.GetDerniersMatchsAsync("5083");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Matches.Clear();
                    TennisMatches.Clear();
                    VolleyMatches.Clear();

                    if (basketResult is not null)
                    {
                        foreach (var match in basketResult)
                        {
                            Matches.Add(match);
                        }
                    }

                    if (tennisResult is not null)
                    {
                        foreach (var match in tennisResult)
                        {
                            TennisMatches.Add(match);
                        }
                    }

                    if (volleyResult is not null)
                    {
                        foreach (var match in volleyResult.Take(5))
                        {
                            VolleyMatches.Add(match);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading scores: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task GoToDetailAsync(MatchModel match)
        {
            if (match is null)
            {
                return;
            }

            var navigationParameter = new Dictionary<string, object>
            {
                { "Match", match }
            };

            try
            {
                await Shell.Current.GoToAsync("ScoreDetailPage", navigationParameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }
    }
}
