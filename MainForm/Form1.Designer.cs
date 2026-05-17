namespace MainForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cbTeams = new System.Windows.Forms.ComboBox();
            this.pnlPlayers = new System.Windows.Forms.Panel();
            this.pnlFavourites = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FormSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.rankListsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblWorldCup = new System.Windows.Forms.Label();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.pnlPlayerDetails = new System.Windows.Forms.Panel();
            this.btnChangeImage = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // cbTeams
            // 
            resources.ApplyResources(this.cbTeams, "cbTeams");
            this.cbTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTeams.FormattingEnabled = true;
            this.cbTeams.Name = "cbTeams";
            this.cbTeams.Sorted = true;
            this.cbTeams.SelectedIndexChanged += new System.EventHandler(this.cbTeams_SelectedIndexChanged);
            // 
            // pnlPlayers
            // 
            resources.ApplyResources(this.pnlPlayers, "pnlPlayers");
            this.pnlPlayers.AllowDrop = true;
            this.pnlPlayers.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlPlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPlayers.Name = "pnlPlayers";
            this.pnlPlayers.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlPlayers_DragDrop);
            this.pnlPlayers.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlPlayers_DragEnter);
            this.pnlPlayers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlPlayers_MouseDoubleClick);
            // 
            // pnlFavourites
            // 
            resources.ApplyResources(this.pnlFavourites, "pnlFavourites");
            this.pnlFavourites.AllowDrop = true;
            this.pnlFavourites.BackColor = System.Drawing.Color.Khaki;
            this.pnlFavourites.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFavourites.Name = "pnlFavourites";
            this.pnlFavourites.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlFavourites_DragDrop);
            this.pnlFavourites.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlFavourites_DragEnter);
            this.pnlFavourites.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlFavourites_MouseDoubleClick);
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FormSettings,
            this.rankListsToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // FormSettings
            // 
            resources.ApplyResources(this.FormSettings, "FormSettings");
            this.FormSettings.Name = "FormSettings";
            this.FormSettings.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // rankListsToolStripMenuItem
            // 
            resources.ApplyResources(this.rankListsToolStripMenuItem, "rankListsToolStripMenuItem");
            this.rankListsToolStripMenuItem.Name = "rankListsToolStripMenuItem";
            this.rankListsToolStripMenuItem.Click += new System.EventHandler(this.rankListsToolStripMenuItem_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Yellow;
            this.label3.Name = "label3";
            // 
            // lblWorldCup
            // 
            resources.ApplyResources(this.lblWorldCup, "lblWorldCup");
            this.lblWorldCup.BackColor = System.Drawing.Color.Transparent;
            this.lblWorldCup.ForeColor = System.Drawing.Color.Yellow;
            this.lblWorldCup.Name = "lblWorldCup";
            // 
            // lblLanguage
            // 
            resources.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.BackColor = System.Drawing.Color.Transparent;
            this.lblLanguage.ForeColor = System.Drawing.Color.Yellow;
            this.lblLanguage.Name = "lblLanguage";
            // 
            // pbLoading
            // 
            resources.ApplyResources(this.pbLoading, "pbLoading");
            this.pbLoading.BackColor = System.Drawing.Color.Transparent;
            this.pbLoading.Image = global::MainForm.Properties.Resources.loading_v2;
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.TabStop = false;
            // 
            // pnlPlayerDetails
            // 
            resources.ApplyResources(this.pnlPlayerDetails, "pnlPlayerDetails");
            this.pnlPlayerDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPlayerDetails.Name = "pnlPlayerDetails";
            // 
            // btnChangeImage
            // 
            resources.ApplyResources(this.btnChangeImage, "btnChangeImage");
            this.btnChangeImage.BackColor = System.Drawing.Color.Gray;
            this.btnChangeImage.ForeColor = System.Drawing.Color.Yellow;
            this.btnChangeImage.Name = "btnChangeImage";
            this.btnChangeImage.UseVisualStyleBackColor = false;
            this.btnChangeImage.Click += new System.EventHandler(this.btnChangeImage_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MainForm.Properties.Resources.world_cup_trophy_fifa_football_3228493;
            this.Controls.Add(this.btnChangeImage);
            this.Controls.Add(this.pnlPlayerDetails);
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.lblWorldCup);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlFavourites);
            this.Controls.Add(this.pnlPlayers);
            this.Controls.Add(this.cbTeams);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTeams;
        private System.Windows.Forms.Panel pnlPlayers;
        private System.Windows.Forms.Panel pnlFavourites;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FormSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblWorldCup;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.Panel pnlPlayerDetails;
        private System.Windows.Forms.Button btnChangeImage;
        private System.Windows.Forms.ToolStripMenuItem rankListsToolStripMenuItem;
    }
}

