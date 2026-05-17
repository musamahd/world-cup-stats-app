using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Interaction logic for TeamStats.xaml
    /// </summary>
    public partial class TeamStats : Window
    {
        private Team team;
       
        public TeamStats(Team teamData)
        {
            InitializeComponent();
            team = teamData; // ✅ Properly assign passed stats
            UpdateTeamUI();
               
        }

        private void UpdateTeamUI()
        {


            tbTeam.Text = team.Country;
            tbFIFAcode.Text =tbFIFAcode.Text;    
            tbMatches.Text = team.MatchesPlayed.ToString();
            tbWins.Text = team.Wins.ToString();
            tbDraws.Text = team.Draws.ToString();
            tbLost.Text = team.Losses.ToString();
            tbScored.Text = team.GoalsScored.ToString();
            tbConceded.Text = team.GoalsConceded.ToString();
            tbDifference.Text = (team.GoalsScored - team.GoalsConceded).ToString(); 
            
            
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2; //  Center horizontally
            this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2; //  Center vertically
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard fadeIn = (Storyboard)FindResource("FadeInAnimation");
            fadeIn.Begin(MainGrid);
        }
    }
}
