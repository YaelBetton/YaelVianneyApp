using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YaelApp.Models;

namespace YaelApp.Services
{
    public class SportsService
    {
        private readonly HttpClient _httpClient;
        
        private const string BaseUrl = "https://www.thesportsdb.com/api/v1/json/123/eventspastleague.php?id=";

        public SportsService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<MatchModel>> GetDerniersMatchsAsync(string leagueId)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(BaseUrl + leagueId);

                // 2. Désérialisation avec Newtonsoft.Json
                var data = JsonConvert.DeserializeObject<SportsResponse>(response);

                // 3. On retourne la liste de matchs
                return data?.Events ?? new List<MatchModel>();
            }
            catch (Exception ex)
            {
                // Gérer l'erreur (ex: pas d'internet)
                return new List<MatchModel>();
            }
        }
    }
}
