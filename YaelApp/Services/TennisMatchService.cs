using System.Collections.ObjectModel;
using YaelApp.Models;

namespace YaelApp.Services
{
    public class TennisMatchService
    {
        private static TennisMatchService? _instance;
        public static TennisMatchService Instance => _instance ??= new TennisMatchService();

        public ObservableCollection<MatchModel> TennisMatches { get; } = new();

        private TennisMatchService() { }

        public void AddMatch(MatchModel match)
        {
            TennisMatches.Insert(0, match);
        }
    }
}

