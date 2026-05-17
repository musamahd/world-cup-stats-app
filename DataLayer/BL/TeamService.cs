using DataLayer.Config;
using DataLayer.Models;
using DataLayer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Match = DataLayer.Models.Match;

namespace DataLayer.BL
{
    public class TeamService
    {
        private readonly ApiService _apiService = new ApiService();
        public async Task<List<Team>> GetTeams(bool isMenWorldCup)
        {
            bool useApi = ConfigManager.UseApi;
            string jsonPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "WorldCupStats", "data", isMenWorldCup ? "men_teams.json" : "women_teams.json"
            );

            if (!File.Exists(jsonPath))
            {
                Debug.WriteLine($" JSON file missing! Fetching from API...");
                List<Team> teams = await _apiService.GetTeamsAsync(isMenWorldCup);

                //  Ensure directory exists before saving
                Directory.CreateDirectory(Path.GetDirectoryName(jsonPath));
                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(teams)); //  Save the new data

                Debug.WriteLine($"JSON file restored at: {jsonPath}");
                return teams;
            }

            Debug.WriteLine($" Loading JSON from file: {jsonPath}");
            string jsonData = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<List<Team>>(jsonData);
        }



        public async Task<List<Player>> GetPlayers(string countryCode,bool isMenWorldCup)
        {
            Debug.WriteLine($"Fetching players for {countryCode} from match data...");
            List<Match> matches = LoadMatches(isMenWorldCup);

            //  Get ONLY the most recent match where the team played
            Match latestMatch = matches
                .Where(m => m.HomeTeam.Country.Equals(countryCode, StringComparison.OrdinalIgnoreCase) ||
                            m.AwayTeam.Country.Equals(countryCode, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(m => m.FifaId) // Ensure it's the latest match
                .FirstOrDefault();

            List<Player> players = new List<Player>();

            if (latestMatch != null)
            {
                Debug.WriteLine($"✅ Using latest match for {countryCode}: {latestMatch.HomeTeam.Country} vs {latestMatch.AwayTeam.Country}");

                if (latestMatch.HomeTeam.Country.Equals(countryCode, StringComparison.OrdinalIgnoreCase))
                {
                    players.AddRange(latestMatch.HomeTeamStatistics?.StartingEleven.Select(player => new Player
                    {
                        Name = player.Name,
                        Position = player.Position ?? "Unknown",
                        ShirtNumber = player.ShirtNumber,
                        Goals = latestMatch.HomeTeamEvents.Count(e => e.TypeOfEvent == "goal" && e.Player == player.Name),
                        YellowCards = latestMatch.HomeTeamEvents.Count(e => e.TypeOfEvent == "yellow-card" && e.Player == player.Name),
                        ImagePath = LoadPlayerImage(player.Name) //  Ensure correct image loading
                    }) ?? new List<Player>());
                }
                else if (latestMatch.AwayTeam.Country.Equals(countryCode, StringComparison.OrdinalIgnoreCase))
                {
                    players.AddRange(latestMatch.AwayTeamStatistics?.StartingEleven.Select(player => new Player
                    {
                        Name = player.Name,
                        Position = player.Position ?? "Unknown",
                        ShirtNumber = player.ShirtNumber,
                        Goals = latestMatch.AwayTeamEvents.Count(e => e.TypeOfEvent == "goal" && e.Player == player.Name),
                        YellowCards = latestMatch.AwayTeamEvents.Count(e => e.TypeOfEvent == "yellow-card" && e.Player == player.Name),
                        ImagePath = LoadPlayerImage(player.Name) //  Ensure correct image loading
                    }) ?? new List<Player>());
                }
            }
            else
            {
                Debug.WriteLine($" No valid matches found for {countryCode}, fetching from API...");
                return await _apiService.GetPlayersAsync(countryCode, isMenWorldCup); //  Ensure API retrieves correct data
            }

            Debug.WriteLine($" FINAL Players Count for {countryCode}: {players.Count}");
            return players;
        }
       

        private string LoadPlayerImage(string playerName)
        {
            string relativePath = Path.Combine("Images", $"{playerName}.jpg"); //  Relative path inside app
            string absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath); // Convert to absolute path

            if (File.Exists(absolutePath))
            {
                return absolutePath; // Use stored image from app's "Images" folder
            }

            return "/Images/Field.jpg";
        }

        public Team GetTeamStats(string countryCode, bool isMenWorldCup)
        {
            Debug.WriteLine($"Fetching team stats for {countryCode}...");

            List<Match> matches = LoadMatches(isMenWorldCup);
            List<Team> results = LoadResults(); // Load results JSON

            Team team = results.FirstOrDefault(t => t.Country.Equals(countryCode, StringComparison.OrdinalIgnoreCase))
                        ?? new Team { Country = countryCode };

            if (team.FIFA_Code != null)
            {
                Debug.WriteLine($" FIFA Code Retrieved: {team.FIFA_Code} for {team.Country}");
            }
            else
            {
                Debug.WriteLine($" No FIFA code found for {countryCode}");
            }

            // ✅ Calculate Stats Without Recursive Calls
            foreach (var match in matches)
            {
                bool isHomeTeam = match.HomeTeam.Country.Equals(countryCode, StringComparison.OrdinalIgnoreCase);
                bool isAwayTeam = match.AwayTeam.Country.Equals(countryCode, StringComparison.OrdinalIgnoreCase);

                if (isHomeTeam || isAwayTeam)
                {
                    team.MatchesPlayed++; // Count match only if the team participated

                    if (match.Winner.Equals(countryCode, StringComparison.OrdinalIgnoreCase))
                        team.Wins++;
                    else if (match.Winner.Equals("Draw", StringComparison.OrdinalIgnoreCase))
                        team.Draws++;
                    else
                        team.Losses++;
                }

                // ✅ Ensure Proper Goal Calculation
                if (isHomeTeam)
                {
                    team.GoalsScored += match.HomeTeam.GoalsScored;
                    team.GoalsConceded += match.AwayTeam.GoalsScored;
                }
                if (isAwayTeam)
                {
                    team.GoalsScored += match.AwayTeam.GoalsScored;
                    team.GoalsConceded += match.HomeTeam.GoalsScored;
                }
            }

            Debug.WriteLine($" Team Stats Calculated: {team.Country} - {team.Wins}W/{team.Draws}D/{team.Losses}L");
            return team;
        }
        public List<Match> LoadMatches(bool isMenWorldCup)
        {
            string matchFileName = isMenWorldCup ? "men_matches.json" : "women_matches.json"; //  Use correct file
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", matchFileName);

            if (!File.Exists(jsonPath))
            {
                Debug.WriteLine($" Matches JSON file NOT FOUND at {jsonPath}! Fetching from API...");

                // Fetch match data dynamically from API
                List<Match> matches = FetchMatchesFromApi(isMenWorldCup); 

                //  Ensure directory exists before saving
                Directory.CreateDirectory(Path.GetDirectoryName(jsonPath));

                //  Save matches to file for future use
                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(matches));

                Debug.WriteLine($" Match data saved at {jsonPath}");

                return matches;
            }

            Debug.WriteLine($" Loading matches from file: {jsonPath}");
            string jsonData = File.ReadAllText(jsonPath);
            List<Match> matchesFromFile = JsonConvert.DeserializeObject<List<Match>>(jsonData);

            foreach (var match in matchesFromFile)
            {
                Debug.WriteLine($"Match {match.FifaId} - {match.HomeTeam.FIFA_Code} vs {match.AwayTeam.FIFA_Code}");
                Debug.WriteLine($"Home Team Events: {match.HomeTeamEvents.Count}");
                Debug.WriteLine($"Away Team Events: {match.AwayTeamEvents.Count}");
            }

            return matchesFromFile;
        }

        private List<Match> FetchMatchesFromApi(bool isMenWorldCup)
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

        public List<Team> LoadResults()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "men_results.json");
            Debug.WriteLine($"Loading results data from {jsonPath}");

            if (!File.Exists(jsonPath))
            {
                Debug.WriteLine("ERROR: Results data file not found!");
                return new List<Team>(); // Return empty list to avoid crash
            }

            string jsonText = File.ReadAllText(jsonPath);
            List<Team> results = JsonConvert.DeserializeObject<List<Team>>(jsonText);

            Debug.WriteLine($"Loaded {results.Count} teams from results data");
            return results;
        }

        public Player GetPlayerStatsForMatch(string playerName, Match selectedMatch)
        {
            if (selectedMatch == null) return new Player(); // Prevent null reference

            // Check if the player is in home or away team & calculate stats
            var homePlayer = selectedMatch.HomeTeamStatistics?.StartingEleven.FirstOrDefault(p => p.Name == playerName);
            var awayPlayer = selectedMatch.AwayTeamStatistics?.StartingEleven.FirstOrDefault(p => p.Name == playerName);

            if (homePlayer != null)
            {
                return new Player
                {
                    Name = homePlayer.Name,
                    Position = homePlayer.Position ?? "Unknown",
                    ShirtNumber = homePlayer.ShirtNumber,
                    Goals = selectedMatch.HomeTeamEvents.Count(e => e.TypeOfEvent == "goal" && e.Player == playerName),
                    YellowCards = selectedMatch.HomeTeamEvents.Count(e => e.TypeOfEvent == "yellow-card" && e.Player == playerName),
                     IsCaptain = homePlayer.IsCaptain
                };
            }

            if (awayPlayer != null)
            {
                return new Player
                {
                    Name = awayPlayer.Name,
                    Position = awayPlayer.Position ?? "Unknown",
                    ShirtNumber = awayPlayer.ShirtNumber,
                    Goals = selectedMatch.AwayTeamEvents.Count(e => e.TypeOfEvent == "goal" && e.Player == playerName),
                    YellowCards = selectedMatch.AwayTeamEvents.Count(e => e.TypeOfEvent == "yellow-card" && e.Player == playerName),
                     IsCaptain = homePlayer.IsCaptain
                };
            }

            return new Player();
        }

    }
}
