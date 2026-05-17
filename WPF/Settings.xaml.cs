using DataLayer.Config;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.Helpers;

namespace WPF
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            LoadSettings();
            
        }

        private void LoadSettings()
        {
            string[] settings = UserSettings.Load();

            bool isMenWorldCup = ConfigManager.UseMenWorldCup;
            cmbWC.SelectedItem = isMenWorldCup ? cmbMen : cmbWomen;

            cmbLanguage.SelectedItem = settings[0] == "en-US" ? cmbEnglish : cmbCroatian;
          //  cmbWC.SelectedItem = settings[1]=="Men"?cmbMen:cmbWomen; // "Men" ili "Women"
        }

        private void SaveSettings()
        {
            string language;
            string selectedWC = cmbWC.Text;
            string newWindowSize = GetSelectedWindowSize();

            if (cmbLanguage.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();

                if (selectedText=="English")
                {
                    language = "en-US";
                }
                else if (selectedText=="Hrvatski")
                {
                    language = "hr-HR";
                }
                else
                {
                    language = "Unknown"; // Fallback value if no match is found
                }
            }
            else
            {
                language = "Unknown"; // Handle case where nothing is selected
            }

            string[] previousSettings = UserSettings.Load();
            string previousWindowSize = previousSettings.Length >= 3 ? previousSettings[2] : "1280x720";
            bool isMenWorldCup = selectedWC.Equals("Men", StringComparison.OrdinalIgnoreCase);
            ConfigManager.SetTournament(isMenWorldCup);
            UserSettings.Save(language, selectedWC,newWindowSize);
           

            //Only close &reopen MainWindow if the size has changed
            if (newWindowSize != previousWindowSize)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow)
                    {
                        window.Close(); // ✅ Close current instance only if size is different
                    }
                }

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            this.Close();
        }

        private string GetSelectedWindowSize()
        {
            if (rb800.IsChecked == true) return "800x600";
            if (rb1280.IsChecked == true) return "1280x720";
            if (rbFullscreen.IsChecked == true) return "1920x1080";

            return "Default"; // Fallback value
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to change the World Cup and language?","Confirm Settings Change",MessageBoxButton.OKCancel,MessageBoxImage.Warning); 

            if (result == MessageBoxResult.OK)
            {
                MessageBox.Show("Settings saved successfully!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                SaveSettings();
                this.Close();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSave.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                e.Handled = true;
            }
            if (e.Key == Key.Escape)
            {
                this.Close();
                e.Handled = true;
            }
        }
    }
}
