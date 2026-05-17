using DataLayer.BL;
using DataLayer.Config;
using DataLayer.Models;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using WPF.Controls;
using WPF.Helpers;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Team> teams = new List<Team>();
        bool isMenWorldCup = ConfigManager.UseMenWorldCup; // Get value from settings
        

        public MainWindow()
        {
            InitializeComponent();
            string[] settings = UserSettings.Load();

            if (settings.Length >= 3) // Ensure window size is available
            {
                ApplyWindowSize(settings[2]); //  Apply saved window size
            }
            ApplySettings(settings); // Apply language & championship selection
        }

        private void ApplyWindowSize(string size)
        {
            switch (size)
            {
                case "Fullscreen":
                    this.WindowState = WindowState.Maximized;
                    break;
                case "1280x720":
                    this.Width = 1280;
                    this.Height = 720;
                    break;
                case "1920x1080":
                    this.Width = 1920;
                    this.Height = 1080;
                    break;
                case "800x600":
                    this.Width = 800;
                    this.Height = 600;
                    break;
                default:
                    this.WindowState = WindowState.Normal;
                    break;
            };
        }

        private void ApplySettings(string[] settings)
        {
            if (settings.Length < 3) return;

            string language = settings[0];
            string worldcup = settings[1];
            string windowSize = settings[2];

            // Apply language 
            CultureInfo culture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            ApplyWindowSize(windowSize);
           
        }

        private void BtnPostavke_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();    
        }

        private void btnHomeStats_Click(object sender, RoutedEventArgs e)
        {
            if (cbHome.SelectedItem is Team selectedTeam)
            {
                TeamService teamService = new TeamService();
                Team teamStats = teamService.GetTeamStats(selectedTeam.Country,isMenWorldCup);
                TeamStats teamStatsWindow = new TeamStats(teamStats); 
                teamStatsWindow.Show(); 
            }
        }

        private async void cbAway_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            TeamService teamService = new TeamService();
            bool isMenWorldCup = ConfigManager.UseMenWorldCup; //  Retrieve the correct setting
            List<Match> matches = teamService.LoadMatches(isMenWorldCup); // Pass the parameter

            Debug.WriteLine("cbAway_SelectionChangedAsync triggered!");

            if (cbAway.SelectedItem == null || cbHome.SelectedItem == null)
            {
                Debug.WriteLine("Home or Away team not selected, exiting...");
                lblHomeScore.Content = "0"; // Reset if no match is selected
                lblAwayScore.Content = "0"; // Reset if no match is selected
                btnAwayStats.IsEnabled = false;
                return;
            }

            Team selectedHomeTeam = cbHome.SelectedItem as Team;
            Team selectedAwayTeam = cbAway.SelectedItem as Team;

            if (selectedHomeTeam == null || selectedAwayTeam == null)
            {
                Debug.WriteLine("ERROR: Selected teams are invalid.");
                return;
            }
            btnAwayStats.IsEnabled = true;

            Debug.WriteLine($"Selected Home Team: {selectedHomeTeam.Country}, Away Team: {selectedAwayTeam.Country}");

           // TeamService teamService = new TeamService();
            List<Player> players = await teamService.GetPlayers(selectedAwayTeam.Country, isMenWorldCup); // Get players from match data

            // Clear old players before adding new ones
            spGKAway.Children.Clear();
            spDEFAway.Children.Clear();
            spMIDAway.Children.Clear();
            spATTAway.Children.Clear();

            foreach (var player in players)
            {
                PlayerControl playerControl = new PlayerControl(player);

                // Assign player to correct StackPanel based on position
                switch (player.Position)
                {
                    case "Goalie": spGKAway.Children.Add(playerControl); break;
                    case "Defender": spDEFAway.Children.Add(playerControl); break;
                    case "Midfield": spMIDAway.Children.Add(playerControl); break;
                    case "Forward": spATTAway.Children.Add(playerControl); break;
                    default: Debug.WriteLine($"Unknown position for {player.Name}"); break;
                }
            }

            Debug.WriteLine("Away players updated!");

            // Load match data and find the correct match
            List<DataLayer.Models.Match> match = teamService.LoadMatches(isMenWorldCup);

            DataLayer.Models.Match selectedMatch = match.FirstOrDefault(m =>
                (m.HomeTeam.Country.Equals(selectedHomeTeam.Country, StringComparison.OrdinalIgnoreCase) &&
                 m.AwayTeam.Country.Equals(selectedAwayTeam.Country, StringComparison.OrdinalIgnoreCase)) ||
                (m.HomeTeam.Country.Equals(selectedAwayTeam.Country, StringComparison.OrdinalIgnoreCase) &&
                 m.AwayTeam.Country.Equals(selectedHomeTeam.Country, StringComparison.OrdinalIgnoreCase)));

            if (selectedMatch != null)
            {
                if (selectedMatch.HomeTeam.Country.Equals(selectedHomeTeam.Country, StringComparison.OrdinalIgnoreCase))
                {
                    lblHomeScore.Content = selectedMatch.HomeTeam.GoalsScored.ToString();
                    lblAwayScore.Content = selectedMatch.AwayTeam.GoalsScored.ToString();
                }
                else
                {
                    lblHomeScore.Content = selectedMatch.AwayTeam.GoalsScored.ToString();
                    lblAwayScore.Content = selectedMatch.HomeTeam.GoalsScored.ToString();
                }
            }
            else
            {
                lblHomeScore.Content = "0";
                lblAwayScore.Content = "0";
                Debug.WriteLine($"No match found for {selectedHomeTeam.Country} vs {selectedAwayTeam.Country}");
            }
        }

        private void btnAwayStats_Click(object sender, RoutedEventArgs e)
        {
            if (cbAway.SelectedItem is Team selectedTeam)
            {
                TeamService teamService = new TeamService();
                Team teamStats = teamService.GetTeamStats(selectedTeam.Country, isMenWorldCup); 

                TeamStats teamStatsWindow = new TeamStats(teamStats); 
                teamStatsWindow.Show(); 
            }
        }

        private void ttlWC_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(" Window Loaded - Fetching Teams!");
            _=LoadTeamsAsync(true);
        }

        private async Task LoadTeamsAsync(bool v)
        {
            Debug.WriteLine("LoadTeamsAsync() started in WPF!");
            string[] settings = UserSettings.Load();
            bool isMenWorldCup = settings.Length >= 2 && settings[1].Equals("Men", StringComparison.OrdinalIgnoreCase);

            TeamService teamService = new TeamService();
            List<Team> teams = await teamService.GetTeams(isMenWorldCup); //  Calls shared logic
            Debug.WriteLine($" {teams.Count} teams fetched from {(ConfigManager.UseApi ? "API" : "JSON")}");

            cbHome.Items.Clear();
            foreach (var team in teams)
            {
                cbHome.Items.Add(team);
                Debug.WriteLine($"Adding team: {team.Country}");
            }
            if (teams.Count > 0)
            {
                cbHome.SelectedIndex = 0; // Auto-select first team
            }
        }

        private async void cbHome_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("cbHome_SelectionChangedAsync triggered!");

            if (cbHome.SelectedItem == null)
            {
                Debug.WriteLine("No team selected, exiting...");
                btnHomeStats.IsEnabled = false;
                return;
            }

            Team selectedTeam = cbHome.SelectedItem as Team;
            if (selectedTeam ==null)
            {
                Debug.WriteLine("ERROR: cbHome.SelectedItem is not a valid Team.");
                return;
            }
            btnHomeStats.IsEnabled = true;

            Debug.WriteLine($"Selected Team: {selectedTeam.Country}");

            TeamService teamService = new TeamService();
            List<Player> players = await teamService.GetPlayers(selectedTeam.Country,isMenWorldCup); // Get players from match data
            
            List<DataLayer.Models.Match> matches = teamService.LoadMatches(isMenWorldCup);

            // Get list of opponents from matches
            var opponentTeams = new HashSet<Team>();

            foreach (var match in matches)
            {
                if (match.HomeTeam.Country.Equals(selectedTeam.Country, StringComparison.OrdinalIgnoreCase))
                {
                    opponentTeams.Add(match.AwayTeam); // Add away team to list
                }

                if (match.AwayTeam.Country.Equals(selectedTeam.Country, StringComparison.OrdinalIgnoreCase))
                {
                    opponentTeams.Add(match.HomeTeam); // Add home team to list
                }
            }

            Debug.WriteLine($"Found {opponentTeams.Count} opponents for {selectedTeam.Country}");

            // Update Away Team combobox
            cbAway.ItemsSource = opponentTeams.ToList(); //  Set filtered opponent list
            cbAway.SelectedIndex = -1; //  Reset selection

            //  Clear old players before adding new ones
            spGKHome.Children.Clear();
            spDEFHome.Children.Clear();
            spMIDHome.Children.Clear();
            spATTHome.Children.Clear();
            spGKAway.Children.Clear();
            spDEFAway.Children.Clear();
            spMIDAway.Children.Clear();
            spATTAway.Children.Clear();

            foreach (var player in players.Where(p => p.Position != "Substitute"))
            {
                PlayerControl playerControl = new PlayerControl(player);
             

                // Assign player to correct StackPanel based on position
                switch (player.Position)
                {
                    case "Goalie": spGKHome.Children.Add(playerControl); break;
                    case "Defender": spDEFHome.Children.Add(playerControl); break;
                    case "Midfield": spMIDHome.Children.Add(playerControl); break;
                    case "Forward": spATTHome.Children.Add(playerControl); break;
                    default: Debug.WriteLine($"Unknown position for {player.Name}"); break;
                }
            }
            
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2; // ✅ Center horizontally
            this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2; // ✅ Center vertically
        }



    }
}