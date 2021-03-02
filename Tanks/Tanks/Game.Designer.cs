
namespace Tanks
{
    partial class Game
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerOfGame = new System.Windows.Forms.Timer(this.components);
            this.txtScore = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timerResumeUpdate = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timerOfGame
            // 
            this.timerOfGame.Interval = 15;
            this.timerOfGame.Tick += new System.EventHandler(this.timerOfGame_Tick);
            // 
            // txtScore
            // 
            this.txtScore.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtScore.ForeColor = System.Drawing.Color.White;
            this.txtScore.Location = new System.Drawing.Point(525, 9);
            this.txtScore.Name = "txtScore";
            this.txtScore.Size = new System.Drawing.Size(160, 82);
            this.txtScore.TabIndex = 1;
            this.txtScore.Text = "Apples:  0";
            this.txtScore.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonStart
            // 
            this.buttonStart.AutoSize = true;
            this.buttonStart.Location = new System.Drawing.Point(564, 122);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 25);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "New Game";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            this.buttonStart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(520, 520);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // timerResumeUpdate
            // 
            this.timerResumeUpdate.Interval = 500;
            this.timerResumeUpdate.Tick += new System.EventHandler(this.timerResumeUpdate_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(575, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 45);
            this.label1.TabIndex = 4;
            this.label1.Text = "  W\r\nASD \r\nB - shoot";
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(685, 521);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.txtScore);
            this.KeyPreview = true;
            this.Name = "Game";
            this.Text = "Battle City";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyIsDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerOfGame;
        private System.Windows.Forms.Label txtScore;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timerResumeUpdate;
        private System.Windows.Forms.Label label1;
    }
}

