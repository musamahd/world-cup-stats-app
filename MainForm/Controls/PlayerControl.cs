using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm.Controls
{
    public partial class PlayerControl : UserControl
    {
        public Player PlayerData { get; private set; }
        public PlayerControl(Player player)
        {
            InitializeComponent();
            PlayerData = player;

            lblName.Text = player.Name;
            lblShirtNumber.Text = $"#{player.ShirtNumber}";
            lblPosition.Text = player.Position;
            pbPlayerImage.Image = LoadPlayerImage(player.Name);
            pbPlayerImage.Refresh();
            pbPlayerImage.SizeMode = PictureBoxSizeMode.StretchImage;
            pbPlayerImage.Visible = true;
            Console.WriteLine($" Created PlayerControl for: {player.Name}");
            // Update favorite status            
            pbStar.Visible = player.IsFavorite;
        }

        private void PlayerControl_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Gold;
        }

        private void PlayerControl_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Khaki;
        }

        private Image LoadPlayerImage(string playerName)
        {
            string imagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PlayerImages", $"{playerName}.jpg"); // ✅ Use a more flexible relative path

            if (File.Exists(imagePath))
            {
                return Image.FromFile(imagePath); 
            }

            return Properties.Resources.worldcup;
        }

        


    }
}
