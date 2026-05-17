using DataLayer.Models;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF.Controls
{
    /// <summary>
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        public Player PlayerData { get; private set; }

        public PlayerControl(Player player)
        {
            InitializeComponent();
            PlayerData = player;
            UpdatePlayerUI();
            
        }

        private void UpdatePlayerUI()
        {
            tbPlayer.Text = PlayerData.Name;
            tbShirtNumber.Text = $"#{PlayerData.ShirtNumber}";



            string imagePath = LoadPlayerImage(PlayerData.Name);
            imgPlayer.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));

            Debug.WriteLine($"Updated UI for {PlayerData.Name}");
        }

        private void btnPlayer_Click(object sender, RoutedEventArgs e)
        {
            PlayerStats playerStatsWindow = new PlayerStats(PlayerData); // Pass selected player data
            playerStatsWindow.Show();
        }

        private string LoadPlayerImage(string playerName)
        {
            string commonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PlayerImages",$"{playerName}.jpg");

           

            if (File.Exists(commonPath))
            {
                return commonPath; // Use the existing uploaded image
            }

            return "/Images/Field.jpg";
        }
      



    }
}
