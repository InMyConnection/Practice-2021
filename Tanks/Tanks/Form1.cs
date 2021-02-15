using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    public partial class Form1 : Form
    {
        bool goLeft = false;
        bool goRight = false;
        bool goUp = false;
        bool goDown = false;
        bool shooting = false;

        int score = 0;

        public Form1()
        {
            InitializeComponent();
            CreateBitmapAtRuntime();
        }

        PictureBox pictureBox1 = new PictureBox();
        public void CreateBitmapAtRuntime()
        {
            pictureBox1.Size = new Size(500, 360);
            this.Controls.Add(pictureBox1);

            //Bitmap map = new Bitmap(490, 360);

            Bitmap image = new Bitmap("Up.png");
            pictureBox1.Image = image;
            pictureBox1.Invalidate();

            //Graphics mapGraphics = Graphics.FromImage(map);

            //mapGraphics.FillRectangle(Brushes.Orange, 20, 20, 450, 20);
            //mapGraphics.FillRectangle(Brushes.Blue, 100, 100, 50, 50);
            //mapGraphics.FillEllipse(Brushes.Yellow, 200, 200, 50, 50);

            //pictureBox1.Image = map;
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                shooting = true;
            }
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                shooting = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
