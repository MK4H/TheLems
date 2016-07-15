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
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxGame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBoxGame
            // 
            this.PictureBoxGame.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxGame.Name = "PictureBoxGame";
            this.PictureBoxGame.Size = new System.Drawing.Size(1280, 768);
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
            this.PictureBoxButtons.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBoxButtons_MouseUp);
            // 
            // HlavniOkno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.PictureBoxButtons);
            this.Controls.Add(this.PictureBoxGame);
            this.Name = "HlavniOkno";
            this.Text = "MK Lemmings";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HlavniOkno_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HlavniOkno_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HlavniOkno_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxGame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxButtons)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBoxGame;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.PictureBox PictureBoxButtons;
    }
}

