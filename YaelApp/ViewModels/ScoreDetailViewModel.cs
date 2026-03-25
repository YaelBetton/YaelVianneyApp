using CommunityToolkit.Mvvm.ComponentModel;
using YaelApp.Models;
namespace YaelApp.ViewModels
{
    [QueryProperty(nameof(Match), "Match")]
    public partial class ScoreDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private MatchModel match;
    }
}
