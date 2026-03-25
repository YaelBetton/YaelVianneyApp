using System.Collections.Generic;
using YaelApp.Models;

namespace YaelApp.ViewModels
{
    public class TennisMatchViewModel
    {
        public string NomMatch { get; }
        public string ImageUrl { get; }
        public string Id { get; }
        public List<string> SetsFormatted { get; }

        public TennisMatchViewModel(MatchModel model)
        {
            NomMatch = model.NomMatch;
            ImageUrl = model.ImageUrl;
            Id = model.Id;
            SetsFormatted = new List<string>();
            if (!string.IsNullOrWhiteSpace(model.ResultatRaw) && !string.IsNullOrWhiteSpace(model.EquipeDomicile) && !string.IsNullOrWhiteSpace(model.EquipeExterieur))
            {
                var setParts = model.ResultatRaw.Split('-');
                int setNum = 1;
                foreach (var set in setParts)
                {
                    var scores = set.Trim().Split('/');
                    if (scores.Length == 2)
                    {
                        SetsFormatted.Add($"Set {setNum} : {model.EquipeDomicile} {scores[0]} - {scores[1]} {model.EquipeExterieur}");
                    }
                    setNum++;
                }
            }
        }
    }
}

