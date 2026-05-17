using DataLayer.BL;
using DataLayer.Config;
using DataLayer.Models;
using DataLayer.Storage;
using MainForm.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    public partial class Form1 : Form
    { 
        private List<Player> players = new List<Player>(); // Stores all players
        private List<Team> teams = new List<Team>(); //  Stores all teams
        private Player currentSelectedPlayer = null; // Stores currently selected player
        private List<Player> favoritePlayers = new List<Player>(); // Stores favorite players
        public Form1()
        {
            InitializeComponent();
            ConfigManager.LoadConfig();
            ApplySettings();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            EnsureDataFilesExist(); // Create missing JSON/config files

            string settingsFilePath = Path.Combine("settings.txt");
            string[] settings = File.Exists(settingsFilePath) ? File.ReadAllLines(settingsFilePath) : new[] { "en-US", "Men" };

            bool isMenWorldCup = settings[1].Equals("Men", StringComparison.OrdinalIgnoreCase);
            _ = LoadTeams(isMenWorldCup);
        }

        private void EnsureDataFilesExist()
        {

            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");

            if (!Directory.Exists(jsonPath))
            {
                Directory.CreateDirectory(jsonPath);
                Console.WriteLine($" Created missing data folder: {jsonPath}");
            }

            string menFile = Path.Combine(jsonPath, "men_matches.json");
            string womenFile = Path.Combine(jsonPath, "women_matches.json");
            string settingsFile = Path.Combine("settings.txt");

            if (!File.Exists(menFile)) File.WriteAllText(menFile, "[]"); // Empty JSON placeholder
            if (!File.Exists(womenFile)) File.WriteAllText(womenFile, "[]");
            if (!File.Exists(settingsFile)) File.WriteAllText(settingsFile, "en-US\nMen");
        }

        public void ApplySettings()
        {
            if (File.Exists("settings.txt"))
            {
                string[] settings = File.ReadAllLines("settings.txt");

                if (settings.Length >= 2)
                {
                    string selectedLanguage = settings[0]; // Read language
                    string selectedWorldCup = settings[1]; // Read championship

                    lblLanguage.Text = $"Language: {selectedLanguage}";
                    lblWorldCup.Text = $"World Cup: {selectedWorldCup}";

                    SetCulture(selectedLanguage); // Apply language settings globally
                    bool isMenWorldCup = selectedWorldCup.Equals("Men", StringComparison.OrdinalIgnoreCase);
                    LoadTeams(isMenWorldCup);
                }
            }
        }

        private void SetCulture(string lang)
        {
            try
            {
                CultureInfo culture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            catch (CultureNotFoundException)
            {
                MessageBox.Show("Invalid language selection.");
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings settingsForm = new FormSettings(); 
            settingsForm.Owner = this; // Links settings form to main form
            settingsForm.ShowDialog(); 
            ApplySettings(); 
        }

        public async Task LoadTeams(bool isMenWorldCup)
        {
            pbLoading.Visible = true; //  Show loading animation

            TeamService teamService = new TeamService();
            List<Team> teams = await teamService.GetTeams(isMenWorldCup);

            if (teams == null || teams.Count == 0)
            {
                MessageBox.Show("⚠ No teams found! Check API or JSON.");
                pbLoading.Visible = false;
                return;
            }

            List<TeamInfo> teamInfoList = teams.Select(t => new TeamInfo
            {
                Country = t.Country,
                Code = t.FIFA_Code
            }).ToList();

            cbTeams.BeginInvoke((MethodInvoker)delegate
            {
                cbTeams.DataSource = teamInfoList;
                cbTeams.DisplayMember = "Country";
                cbTeams.Refresh();
                pbLoading.Visible = false; //  Hide loading when UI is updated
            });
        }

        public List<Match> LoadMatches(bool isMenWorldCup)
        {
            string jsonFile = isMenWorldCup ? "men_matches.json" : "women_matches.json";
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", jsonFile);

            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($" Matches JSON file NOT FOUND at {jsonPath}, fetching from API...");
                return FetchMatchesFromApi(isMenWorldCup);
            }

            List<Match> matches = JsonFileManager.LoadMatchesFromJson(jsonPath);
            if (matches.Count == 0)
            {
                Console.WriteLine($" No matches found in {jsonFile}, fetching from API...");
                return FetchMatchesFromApi(isMenWorldCup);
            }

            Console.WriteLine($" Loaded {matches.Count} matches for {(isMenWorldCup ? "Men" : "Women")}");
            return matches;
        }

        private List<Match> FetchMatchesFromApi(bool isMenWorldCup)
        {
            string apiUrl = isMenWorldCup
         ? "http://worldcup-vua.nullbit.hr/men/matches"
         : "http://worldcup-vua.nullbit.hr/women/matches";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                if (!response.IsSuccessStatusCode) return new List<Match>();

                string jsonData = response.Content.ReadAsStringAsync().Result;
                List<Match> matches = JsonConvert.DeserializeObject<List<Match>>(jsonData);

                if (matches.Count > 0)
                {
                    string jsonFile = isMenWorldCup ? "men_matches.json" : "women_matches.json";
                    string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", jsonFile);
                    File.WriteAllText(jsonPath, jsonData); // Save fetched matches for future use
                }

                return matches;
            }
        }

        public List<Player> LoadPlayers(string teamCountry,bool isMenWorldCup)
        {
            List<Match> matches = LoadMatches(isMenWorldCup); 
            HashSet<string> uniquePlayerNames = new HashSet<string>(); 
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
                        if (uniquePlayerNames.Add(player.Name)) // ✅ Add only if name is unique
                        {
                            filteredPlayers.Add(player);
                        }
                    }
                }
            }

            Console.WriteLine($"Found {filteredPlayers.Count} unique players for {teamCountry}");
            return filteredPlayers;

        }

        private void cbTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("EVENT TRIGGERED: Team selection changed!");

            var selectedObject = cbTeams.SelectedItem;
            if (selectedObject is TeamInfo selectedTeam)
            {
                if (string.IsNullOrEmpty(selectedTeam.Country))
                {
                    Console.WriteLine("ERROR: Selected team's Country is NULL or Empty!");
                    return;
                }

                string teamCountry = selectedTeam.Country.Trim();
                Console.WriteLine($"Selected Team: {teamCountry}");

                // ✅ Get tournament selection dynamically
                bool isMenWorldCup = ConfigManager.UseMenWorldCup;

                List<Player> players = LoadPlayers(teamCountry, isMenWorldCup);
                Console.WriteLine($"Loaded {players.Count} players for {teamCountry}");

                FillPlayers(players);
            }
            else
            {
                Console.WriteLine("ERROR: Selected item is NOT a TeamInfo object!");
            }

        }
        
        private void FillPlayers(List<Player> players)
        {
            pnlPlayers.Controls.Clear(); // Remove all previous controls
            int yOffset = 5;
            foreach (Player player in players)
            { 
                Label playerLabel = new Label
                {
                    Text = player.Name, 
                    AutoSize = true,
                    Font = new Font("Arial", 7, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(5, yOffset), // Place labels vertically
                    Cursor = Cursors.Hand,
                    Tag = player
                };
                playerLabel.MouseDown += (s, e) => DisplayPlayerDetails(player);
                playerLabel.MouseDown += (s, e) => StartDragDrop(s as Label);// Show details on click
                pnlPlayers.Controls.Add(playerLabel);
                yOffset += 25; //  Move next label downward
                Console.WriteLine($"Added player name: {player.Name}");
            }
            Console.WriteLine($"Total player names displayed: {players.Count}");
        }

        private void StartDragDrop(Label label)
        {
            if (label == null) return;
            DoDragDrop(label, DragDropEffects.Move);
        }

        private void pnlFavourites_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Label)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void pnlFavourites_DragDrop(object sender, DragEventArgs e)
        {
            Label label = e.Data.GetData(typeof(Label)) as Label;
            if (label != null && favoritePlayers.Count < 3)
            {
                favoritePlayers.Add(label.Tag as Player); 
                pnlPlayers.Controls.Remove(label); 
                pnlFavourites.Controls.Add(label); 
                label.ForeColor = Color.Blue;
                label.MouseDoubleClick += pnlFavourites_MouseDoubleClick;
            }
            if (favoritePlayers.Count > 3)
            {
                MessageBox.Show("⚠ You can only select up to 3 favorite players!", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void pnlFavourites_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender is Label label && label.Tag is Player player)
            {
                player.IsFavorite = false; // Unmark favorite status
                favoritePlayers.Remove(player); // Remove from favorites list

                pnlFavourites.Controls.Remove(label); // Remove from favorites panel
                pnlPlayers.Controls.Add(label); // Move back to players panel

                // Reset color & attach correct event for re-selection
                label.ForeColor = Color.Black;
                label.MouseDoubleClick -= pnlFavourites_MouseDoubleClick;
                label.MouseDoubleClick += pnlPlayers_MouseDoubleClick;
            }
        }

        private void pnlPlayers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender is Label label && label.Tag is Player player)
            {
                if (favoritePlayers.Count < 3)
                {
                    player.IsFavorite = true;
                    favoritePlayers.Add(player);

                    pnlPlayers.Controls.Remove(label);
                    pnlFavourites.Controls.Add(label);

                    label.ForeColor = Color.Blue;
                    label.MouseDoubleClick -= pnlPlayers_MouseDoubleClick;
                    label.MouseDoubleClick += pnlFavourites_MouseDoubleClick;

                    Console.WriteLine($"Added favorite: {player.Name}");
                }
                else
                {
                    MessageBox.Show("You can only select up to 3 favorite players!");
                }
            }
        }

        private void pnlPlayers_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Label)))
            {
                e.Effect = DragDropEffects.Move; // ✅ Allow drop action
            }
        }

        private void pnlPlayers_DragDrop(object sender, DragEventArgs e)
        {
            Label label = e.Data.GetData(typeof(Label)) as Label;
            if (label != null && label.Tag is Player player)
            {
                player.IsFavorite = false; 
                favoritePlayers.Remove(player); 

                pnlFavourites.Controls.Remove(label); 
                pnlPlayers.Controls.Add(label); 

                label.ForeColor = Color.Black; 
                label.MouseDown += (s, ev) => StartDragDrop(label); // ✅ Allow re-dragging

                Console.WriteLine($"❌ Removed favorite: {player.Name}");
            }
        }

        private void DisplayPlayerDetails(Player player)
        {
            currentSelectedPlayer = player;
            pnlPlayerDetails.Controls.Clear(); // Clear previous player detail
            PlayerControl playerControl = new PlayerControl(player);
            pnlPlayerDetails.Controls.Add(playerControl);
            
        }

        private void btnChangeImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Choose Player Image",
                Filter = "Image Files|*.jpg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;

                // Define common path for storing player images
                string commonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PlayerImages");
                // Ensure directory exists
                if (!Directory.Exists(commonPath))
                {
                    Directory.CreateDirectory(commonPath);
                }
                // Construct file path for the player image
                string playerImagePath = Path.Combine(commonPath, $"{currentSelectedPlayer.Name}.jpg");

                if (File.Exists(playerImagePath))
                {
                    try
                    {
                        using (var stream = new FileStream(playerImagePath, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            stream.Close(); // Explicitly close the file before copying
                        }
                    }
                    catch (IOException ex)
                    {
                        Debug.WriteLine($" Warning: File is in use. Retrying copy after release. Error: {ex.Message}");
                    }
                }

                // Proceed with copying the new image after ensuring file access is released
                File.Copy(selectedFilePath, playerImagePath, true);

                DisplayPlayerDetails(currentSelectedPlayer);
               
               
            }
        }
       
        private void rankListsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Players count: {players.Count}, Teams count: {teams.Count}");
            RankingForm rankingForm = new RankingForm(players, teams);
            rankingForm.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
