using DataLayer.Models;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace WPF
{
    /// <summary>
    /// Interaction logic for PlayerStats.xaml
    /// </summary>
    public partial class PlayerStats : Window
    {
        private Player player;
        public PlayerStats(Player selectedPlayer)
        {
            
            InitializeComponent();
            player = selectedPlayer;
            UpdatePlayerUI();
        }

        private void UpdatePlayerUI()
        {
            

            tbName.Text = player.Name;
            tbNumber.Text = $"#{player.ShirtNumber}";
            tbPosition.Text = player.Position;
            tbCaptain.Text = player.IsCaptain ? "Yes" : "No";
            tbGoals.Text = player.Goals.ToString();
            tbYellowCards.Text = player.YellowCards.ToString();

            string imagePath = LoadPlayerImage(player.Name); // Load correct player image
            imgPlayer.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }
       

        private string LoadPlayerImage(string playerName)
        {
            string commonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PlayerImages", $"{playerName}.jpg");



            if (File.Exists(commonPath))
            {
                return commonPath; // Use the existing uploaded image
            }

            return "/Images/Field.jpg";
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2; // Center horizontally
            this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2; //  Center vertically
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(" Player Stats Window Loaded!");

            Storyboard slideIn = (Storyboard)FindResource("SlideInAnimation");
            slideIn.Begin(MainBorder);
        }
    }
}
