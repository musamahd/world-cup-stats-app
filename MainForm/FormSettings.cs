using DataLayer.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MainForm
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            this.KeyPreview = true;
            LoadSettings();
        }

        private readonly string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.txt");

        private void LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                string[] settings = File.ReadAllLines(settingsFilePath);

                if (settings.Length >= 2)
                {
                    cbLanguage.SelectedItem = settings[0]; // Load saved language
                    cbWorldCup.SelectedItem = settings[1]; // Load saved WC
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string selectedCup = cbWorldCup.SelectedItem.ToString();
            string selectedLang = cbLanguage.SelectedItem.ToString() == "English" ? "en-US" : "hr-HR";

            bool isMenWorldCup = selectedCup.Equals("Men", StringComparison.OrdinalIgnoreCase);
            ConfigManager.SetTournament(isMenWorldCup);

            // Use relative path for settings file
            string settingsFilePath = Path.Combine("settings.txt");
            File.WriteAllLines(settingsFilePath, new string[] { selectedLang, selectedCup });

            DialogResult result = MessageBox.Show("Are you sure you want to change the championship and language?",
                "Confirm Settings Change", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                MessageBox.Show("Settings saved successfully!");
            }

            this.Close();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            cbWorldCup.Items.Add("Men");
            cbWorldCup.Items.Add("Women");

            cbLanguage.Items.Add("English");
            cbLanguage.Items.Add("Croatian");

            LoadExistingSettings(); // Load saved preferences
        }

        private void LoadExistingSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                string[] settings = File.ReadAllLines(settingsFilePath);
                cbLanguage.SelectedItem = settings[0] == "en-US" ? "English" : "Croatian";
                cbWorldCup.SelectedItem = settings[1];
            }
        }

        private void FormSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick(); // Apply settings with "Enter"
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close(); // Cancel and close settings with "Esc"
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
