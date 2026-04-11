using System.Collections.ObjectModel;
using YaelApp.Models;

namespace YaelApp.Services
{
    public class VolleyMatchService
    {
        private static VolleyMatchService? _instance;
        public static VolleyMatchService Instance => _instance ??= new VolleyMatchService();

        public ObservableCollection<MatchModel> LocalMatches { get; } = new();

        private VolleyMatchService() { }

        public void AddMatch(MatchModel match)
        {
            LocalMatches.Insert(0, match);
        }
    }
}
