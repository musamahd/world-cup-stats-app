using DataLayer.Config;
using DataLayer.Models;
using DataLayer.Services;
using DataLayer.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    public partial class RankingForm : Form
    {

        public RankingForm(List<Player> playersList, List<Team> teamsList)
        {
            InitializeComponent();
           

        }
        private void RankingForm_Load(object sender, EventArgs e)
        {
            string[] settings = File.Exists("settings.txt") ? File.ReadAllLines("settings.txt") : null;
            string selectedWorldCup = settings != null && settings.Length >= 2 ? settings[1] : "Men"; // Default to Men's World Cup

            bool isMenWorldCup = selectedWorldCup.Equals("Men", StringComparison.OrdinalIgnoreCase);
           _=LoadTeams(isMenWorldCup);
        }

        public async Task LoadTeams(bool isMenWorldCup)
        {
            ApiService apiService = new ApiService();
            List<Team> teams;

            // Read championship from settings.txt
            string[] settings = File.Exists("settings.txt") ? File.ReadAllLines("settings.txt") : null;
            string selectedWorldCup = settings != null && settings.Length >= 2 ? settings[1] : "Men"; // Default to Men's World Cup

            string jsonFile = selectedWorldCup.Equals("Men", StringComparison.OrdinalIgnoreCase) ? "men_teams.json" : "women_teams.json";
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", jsonFile);

            Console.WriteLine($"Selected Championship: {selectedWorldCup}");
            Console.WriteLine($"Loading JSON file: {jsonPath}");

            if (ConfigManager.UseApi)
            {
                Console.WriteLine($"Fetching teams from API ({selectedWorldCup} World Cup)...");
                teams = await apiService.GetTeamsAsync(selectedWorldCup.Equals("Men", StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                if (!File.Exists(jsonPath))
                {
                    Console.WriteLine($"ERROR: JSON file NOT FOUND! Expected at: {jsonPath}");
                    MessageBox.Show($"JSON file missing! Please check the file location: {jsonPath}");
                    return;
                }

                teams = JsonFileManager.LoadTeamsFromJson(jsonPath);

            }



            if (teams != null && teams.Count > 0)
            {
                List<TeamInfo> teamInfoList = teams.Select(t => new TeamInfo
                {
                    Country = t.Country,
                    Code = t.FIFA_Code // ✅ Ensure this matches your JSON mapping!
                }).ToList();


                this.BeginInvoke((MethodInvoker)delegate
                {
                    cbTeams.DataSource = teamInfoList; // ✅ Use TeamInfo objects, not strings!
                    cbTeams.DisplayMember = "Country";
                    cbTeams.Refresh();
                    Console.WriteLine($"✅ UI Updated: {cbTeams.Items.Count} teams displayed.");
                });

            }
        }

        public List<Player> LoadPlayers(string teamCountry)
        {
            List<Match> matches = LoadMatches(); // Load all matches
            HashSet<string> uniquePlayerNames = new HashSet<string>(); // Keep track of unique names
            List<Player> filteredPlayers = new List<Player>();

            Console.WriteLine($"Filtering players for team: {teamCountry}");

            foreach (var match in matches)
            {
                // Filter Home Team Players
                if (match.HomeTeamStatistics != null && string.Equals(match.HomeTeam?.Country, teamCountry, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var player in match.HomeTeamStatistics.StartingEleven)
                    {
                        if (uniquePlayerNames.Add(player.Name)) // Add only if name is unique
                        {
                            filteredPlayers.Add(player);
                        }
                    }
                }

                // Filter Away Team Players
                if (match.AwayTeamStatistics != null && string.Equals(match.AwayTeam?.Country, teamCountry, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var player in match.AwayTeamStatistics.StartingEleven)
                    {
                        if (uniquePlayerNames.Add(player.Name)) // Add only if name is unique
                        {
                            filteredPlayers.Add(player);
                        }
                    }
                }
            }

            Console.WriteLine($"Found {filteredPlayers.Count} unique players for {teamCountry}");
            return filteredPlayers;
        }

        public List<Match> LoadMatches()
        {
            string[] settings = File.Exists("settings.txt") ? File.ReadAllLines("settings.txt") : null;
            string selectedWorldCup = settings != null && settings.Length >= 2 ? settings[1] : "Men"; // Default to Men's World Cup

            string jsonFile = selectedWorldCup.Equals("Men", StringComparison.OrdinalIgnoreCase) ? "men_matches.json" : "women_matches.json";
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", jsonFile);

            Console.WriteLine($"Trying to load {selectedWorldCup} matches from: {jsonPath}");

            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"ERROR: Matches JSON file NOT FOUND at {jsonPath}");
                return new List<Match>(); // Return empty list to avoid crashes
            }
            List<Match> matches = JsonFileManager.LoadMatchesFromJson(jsonPath);

            Console.WriteLine($"Loaded {matches.Count} matches for {selectedWorldCup}");

            return matches;
        }

        private void cbTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedObject = cbTeams.SelectedItem;
            if (selectedObject is TeamInfo selectedTeam)
            {
                string teamCountry = selectedTeam.Country.Trim(); // Use Country Name instead
                List<Player> players = LoadPlayers(teamCountry); //  Send Country, not Code
                FillPlayers(players);
            }
        }

        private void FillPlayers(List<Player> players)
        {
            lstAttendance.Controls.Clear();
            lstMostBooked.Controls.Clear();
            lstTopScorers.Controls.Clear();
            
            
            int yOffset = 5;
            foreach (Player player in players)
            {
                Label AttendanceLabel = new Label
                {
                    Text = player.Name,
                    AutoSize = true,
                    Font = new Font("Arial", 7, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(5, yOffset), // Place labels vertically
                    Cursor = Cursors.Hand,
                    Tag = player
                };Label BookedLabel = new Label
                {
                    Text = player.Name, 
                    AutoSize = true,
                    Font = new Font("Arial", 7, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(5, yOffset), // Place labels vertically
                    Cursor = Cursors.Hand,
                    Tag = player
                };
                Label TopScorersLabel = new Label
                {
                    Text = player.Name, // Display only the name
                    AutoSize = true,
                    Font = new Font("Arial", 7, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(5, yOffset), // Place labels vertically
                    Cursor = Cursors.Hand,
                    Tag = player
                };
                lstAttendance.Controls.Add(AttendanceLabel);
                lstTopScorers.Controls.Add(TopScorersLabel);
                lstMostBooked.Controls.Add(BookedLabel);
                
                yOffset += 25; // Move next label downward
            }
      
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog.ShowDialog();   
        }

        private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void printDocument_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
