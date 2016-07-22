namespace TheLems
{
    partial class HlavniOkno
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
            this.components = new System.ComponentModel.Container();
            this.PictureBoxGame = new System.Windows.Forms.PictureBox();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.PictureBoxButtons = new System.Windows.Forms.PictureBox();
            this.PictureBoxText = new System.Windows.Forms.PictureBox();
            this.PictureBoxMap = new System.Windows.Forms.PictureBox();
            this.PictureBoxMenu = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxGame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBoxGame
            // 
            this.PictureBoxGame.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxGame.Name = "PictureBoxGame";
            this.PictureBoxGame.Size = new System.Drawing.Size(1280, 588);
            this.PictureBoxGame.TabIndex = 0;
            this.PictureBoxGame.TabStop = false;
            this.PictureBoxGame.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBoxGame_MouseMove);
            this.PictureBoxGame.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBoxGame_MouseUp);
            // 
            // Timer
            // 
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // PictureBoxButtons
            // 
            this.PictureBoxButtons.Location = new System.Drawing.Point(0, 774);
            this.PictureBoxButtons.Name = "PictureBoxButtons";
            this.PictureBoxButtons.Size = new System.Drawing.Size(860, 258);
            this.PictureBoxButtons.TabIndex = 1;
            this.PictureBoxButtons.TabStop = false;
            this.PictureBoxButtons.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PictureBoxButtons_MouseDoubleClick);
            this.PictureBoxButtons.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBoxButtons_MouseUp);
            // 
            // PictureBoxText
            // 
            this.PictureBoxText.Location = new System.Drawing.Point(0, 594);
            this.PictureBoxText.Name = "PictureBoxText";
            this.PictureBoxText.Size = new System.Drawing.Size(860, 174);
            this.PictureBoxText.TabIndex = 2;
            this.PictureBoxText.TabStop = false;
            // 
            // PictureBoxMap
            // 
            this.PictureBoxMap.Location = new System.Drawing.Point(866, 594);
            this.PictureBoxMap.Name = "PictureBoxMap";
            this.PictureBoxMap.Size = new System.Drawing.Size(414, 449);
            this.PictureBoxMap.TabIndex = 3;
            this.PictureBoxMap.TabStop = false;
            this.PictureBoxMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMap_MouseClick);
            // 
            // PictureBoxMenu
            // 
            this.PictureBoxMenu.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxMenu.Name = "PictureBoxMenu";
            this.PictureBoxMenu.Size = new System.Drawing.Size(1212, 884);
            this.PictureBoxMenu.TabIndex = 4;
            this.PictureBoxMenu.TabStop = false;
            // 
            // HlavniOkno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.PictureBoxMenu);
            this.Controls.Add(this.PictureBoxMap);
            this.Controls.Add(this.PictureBoxText);
            this.Controls.Add(this.PictureBoxButtons);
            this.Controls.Add(this.PictureBoxGame);
            this.Name = "HlavniOkno";
            this.Text = "MK Lemmings";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HlavniOkno_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HlavniOkno_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HlavniOkno_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxGame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBoxGame;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.PictureBox PictureBoxButtons;
        private System.Windows.Forms.PictureBox PictureBoxText;
        private System.Windows.Forms.PictureBox PictureBoxMap;
        private System.Windows.Forms.PictureBox PictureBoxMenu;
    }
}

