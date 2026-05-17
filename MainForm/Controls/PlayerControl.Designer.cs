namespace MainForm.Controls
{
    partial class PlayerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblShirtNumber = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pbStar = new System.Windows.Forms.PictureBox();
            this.pbPlayerImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbStar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(142, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name: ";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(214, 61);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(30, 18);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "CR";
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosition.Location = new System.Drawing.Point(214, 88);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(63, 18);
            this.lblPosition.TabIndex = 4;
            this.lblPosition.Text = "Forward";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(142, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "Position: ";
            // 
            // lblShirtNumber
            // 
            this.lblShirtNumber.AutoSize = true;
            this.lblShirtNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShirtNumber.Location = new System.Drawing.Point(214, 118);
            this.lblShirtNumber.Name = "lblShirtNumber";
            this.lblShirtNumber.Size = new System.Drawing.Size(16, 18);
            this.lblShirtNumber.TabIndex = 6;
            this.lblShirtNumber.Text = "7";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(142, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 18);
            this.label4.TabIndex = 5;
            this.label4.Text = "Shirt No.: ";
            // 
            // pbStar
            // 
            this.pbStar.Enabled = false;
            this.pbStar.Image = global::MainForm.Properties.Resources.Favorit;
            this.pbStar.Location = new System.Drawing.Point(236, 3);
            this.pbStar.Name = "pbStar";
            this.pbStar.Size = new System.Drawing.Size(41, 42);
            this.pbStar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbStar.TabIndex = 7;
            this.pbStar.TabStop = false;
            // 
            // pbPlayerImage
            // 
            this.pbPlayerImage.BackColor = System.Drawing.Color.Transparent;
            this.pbPlayerImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbPlayerImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPlayerImage.InitialImage = null;
            this.pbPlayerImage.Location = new System.Drawing.Point(3, 3);
            this.pbPlayerImage.Name = "pbPlayerImage";
            this.pbPlayerImage.Size = new System.Drawing.Size(119, 157);
            this.pbPlayerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPlayerImage.TabIndex = 0;
            this.pbPlayerImage.TabStop = false;
            // 
            // PlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.Controls.Add(this.pbStar);
            this.Controls.Add(this.lblShirtNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbPlayerImage);
            this.Name = "PlayerControl";
            this.Size = new System.Drawing.Size(464, 216);
            this.MouseEnter += new System.EventHandler(this.PlayerControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.PlayerControl_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pbStar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayerImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPlayerImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblShirtNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pbStar;
    }
}
