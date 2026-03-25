using System.Collections.Generic;
using Newtonsoft.Json;

namespace YaelApp.Models
{
    public class MatchModel
    {
        [JsonProperty("idEvent")]
        public string Id { get; set; }

        [JsonProperty("strEvent")]
        public string NomMatch { get; set; }

        [JsonProperty("intHomeScore")]
        public string ScoreHomeRaw { get; set; }

        [JsonProperty("intAwayScore")]
        public string ScoreAwayRaw { get; set; }

        [JsonProperty("strResult")]
        public string ResultatRaw { get; set; }

        [JsonProperty("strHomeTeam")]
        public string EquipeDomicile { get; set; }

        [JsonProperty("strAwayTeam")]
        public string EquipeExterieur { get; set; }

        [JsonProperty("strSport")]
        public string Sport { get; set; }

        [JsonIgnore]
        public string ScoreHome => string.IsNullOrEmpty(ScoreHomeRaw) ? (string.IsNullOrEmpty(ResultatRaw) ? "Score indisponible" : ResultatRaw) : ScoreHomeRaw;

        [JsonIgnore]
        public string ScoreAway => string.IsNullOrEmpty(ScoreAwayRaw) ? "" : ScoreAwayRaw;

        [JsonProperty("strThumb")]
        public string RawImageUrl { get; set; }

        [JsonIgnore]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(RawImageUrl))
                {
                    return RawImageUrl;
                }

                return string.Equals(Sport, "Tennis", System.StringComparison.OrdinalIgnoreCase)
                    ? "tennis_fallback.jpg"
                    : "dotnet_bot.png";
            }
        }

        [JsonIgnore]
        public List<string> TennisSetsFormatted
        {
            get
            {
                var sets = new List<string>();
                // Pour les matchs locaux, ResultatRaw est du type "6/3 - 6/2 - 7/5" etc.
                if (!string.IsNullOrWhiteSpace(ResultatRaw) && !string.IsNullOrWhiteSpace(EquipeDomicile) && !string.IsNullOrWhiteSpace(EquipeExterieur))
                {
                    var setParts = ResultatRaw.Split('-');
                    int setNum = 1;
                    foreach (var set in setParts)
                    {
                        var scores = set.Trim().Split('/');
                        if (scores.Length == 2)
                        {
                            sets.Add($"Set {setNum} : {EquipeDomicile} {scores[0]} - {scores[1]} {EquipeExterieur}");
                        }
                        setNum++;
                    }
                }
                return sets;
            }
        }
    }

    // L'API renvoie toujours une liste dans un objet parent (ici "events")
    public class SportsResponse
    {
        [JsonProperty("events")]
        public List<MatchModel> Events { get; set; }
    }
}
