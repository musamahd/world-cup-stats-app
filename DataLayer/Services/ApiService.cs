using DataLayer.Config;
using DataLayer.Models;
using DataLayer.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class ApiService
    {
        private static readonly HttpClient client = new HttpClient();
        public const string MEN_TEAMS = "https://worldcup-vua.nullbit.hr/men/teams/results";
        public const string WOMEN_TEAMS = "https://worldcup-vua.nullbit.hr/women/teams/results";
        public const string MEN_MATCHES = "https://worldcup-vua.nullbit.hr/men/matches";
        public const string WOMEN_MATCHES = "https://worldcup-vua.nullbit.hr/women/matches";
        public const string MEN_MATCHES_BY_TEAM = "https://worldcup-vua.nullbit.hr/men/matches/country?fifa_code={0}";
        public const string WOMEN_MATCHES_BY_TEAM = "https://worldcup-vua.nullbit.hr/women/matches/country?fifa_code={0}";

        public async Task<List<Team>> GetTeamsAsync(bool isMenWorldCup)
        {
            if (!ConfigManager.UseApi)
            {
                string filePath = isMenWorldCup ? "men_teams.json" : "women_teams.json";
                return JsonFileManager.LoadTeamsFromJson(filePath);
            }

            string apiUrl = isMenWorldCup
                ? "http://worldcup-vua.nullbit.hr/men/teams/results"
                : "http://worldcup-vua.nullbit.hr/women/teams/results";

            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Team>>(json);
            }
            

            return null;
        }

       

        public async Task<List<Player>> GetPlayersAsync(string countryCode,bool isMenWorldCup)
        {
            string apiUrl = isMenWorldCup
        ? $"http://worldcup-vua.nullbit.hr/men/players?country={countryCode}"
        : $"http://worldcup-vua.nullbit.hr/women/players?country={countryCode}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"⚠ API request failed: {response.StatusCode}");
                    return new List<Player>(); // Prevent crash
                }

                string jsonData = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(jsonData) || jsonData.TrimStart().StartsWith("<"))
                {
                    Debug.WriteLine($"Unexpected response format (likely HTML error)");
                    return new List<Player>(); //  Prevent parsing invalid JSON
                }

                return JsonConvert.DeserializeObject<List<Player>>(jsonData); //  Ensure correct deserialization
            }
        
        }

        public List<Match> FetchMatchesFromApi(bool isMenWorldCup)
        {
            string apiUrl = isMenWorldCup
                ? "http://worldcup-vua.nullbit.hr/men/matches"
                : "http://worldcup-vua.nullbit.hr/women/matches";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($" API request failed: {response.StatusCode}");
                    return new List<Match>(); //  Prevent crash if API fails
                }

                string jsonData = response.Content.ReadAsStringAsync().Result;

                if (string.IsNullOrWhiteSpace(jsonData) || jsonData.TrimStart().StartsWith("<"))
                {
                    Debug.WriteLine($" Unexpected response format (likely HTML error)");
                    return new List<Match>(); // Avoid parsing invalid data
                }

                return JsonConvert.DeserializeObject<List<Match>>(jsonData);
            }
        }



    }
}
