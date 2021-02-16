using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tanks.Entities;

namespace Tanks
{
    public partial class Form1 : Form
    {
        Kolobok player;
        Tank[] tanks;
        Apple[] apples;
        BrickWall[] walls;
        Graphics g;
        Bitmap b;

        bool shooting = false;
        int score = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = b;
            player = new Kolobok(new Point(100, 100));
            tanks = new Tank[5];
            for (int i = 0; i < 5; i++)
            {
                tanks[i] = new Tank();
            }
            apples = new Apple[5];
            for (int i = 0; i < 5; i++)
            {
                apples[i] = new Apple();
            }
            walls = new BrickWall[3];
            walls[0] = new BrickWall(new Point(50, 50), 270, 40);
            walls[1] = new BrickWall(new Point(50, 140), 40, 200);
            walls[2] = new BrickWall(new Point(140, 160), 190, 40);
            primaryTimer.Enabled = true;
            tankDirectionSwitch.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                player.Move();
                player.Render(g);
                for (int i = 0; i < tanks.Length; i++)
                {
                    tanks[i].Move();
                    tanks[i].Render(g);
                }
                for (int i = 0; i < apples.Length; i++)
                {
                    apples[i].Render(g);
                }
                for (int i = 0; i < walls.Length; i++)
                {
                    walls[i].Render(g);
                }
            }
            pictureBox1.Image = b;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < tanks.Length; i++)
            {
                tanks[i].GetDirection();
            }
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                player.direction = Direction.LEFT;
            }
            if (e.KeyCode == Keys.D)
            {
                player.direction = Direction.RIGHT;
            }
            if (e.KeyCode == Keys.W)
            {
                player.direction = Direction.UP;
            }
            if (e.KeyCode == Keys.S)
            {
                player.direction = Direction.DOWN;
            }
            if (e.KeyCode == Keys.Space)
            {
                shooting = true;
            }
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                shooting = false;
            }
        }




        /*public void Draw()
        {
            pictureBox1.Size = new Size(400, 400);
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pictureBox1);

            var b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = b;

            using (var g = Graphics.FromImage(b))
            {
                using (var b1 = new Bitmap(@"Images\Apple.png")) g.DrawImage(b1, new Point(30, 30));
                using (var b2 = new Bitmap(@"Images\Kolobok.png")) g.DrawImage(b2, new Point(90, 90));
                using (var b3 = new Bitmap(@"Images\Bullet.png")) g.DrawImage(b3, new Point(130, 130));
            }

            pictureBox1.Invalidate();
        }

        public void DrawImage()
        {
            pictureBox1.Size = new Size(400, 400);
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pictureBox1);

            Bitmap map = new Bitmap(380, 380);

            Graphics g = Graphics.FromImage(map);
            // Create image.
            Image newImage = Image.FromFile("Up.png");

            // Create coordinates for upper-left corner of image.
            int x = 100;
            int y = 100;

            // Create rectangle for source image.
            //RectangleF srcRect = new RectangleF(50, 50, 150, 150);
            //GraphicsUnit units = GraphicsUnit.Pixel;

            // Draw image to screen.
            g.FillRectangle(Brushes.Orange, 40, 40, 320, 40);
            g.FillRectangle(Brushes.Orange, 40, 120, 40, 200);
            g.FillRectangle(Brushes.Orange, 140, 160, 220, 40);
            g.DrawImage(newImage, x, y);
            g.DrawImage(newImage, 200, 80);
            g.DrawImage(newImage, 340, 340);

            pictureBox1.Image = map;
        }


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

            /*Bitmap b = new Bitmap(300, 300);
            
            using (Graphics g = Graphics.FromImage(b))
            {
                g.FillRectangle(Brushes.Transparent, 0, 0, b.Width, b.Height);
                g.DrawEllipse(new Pen(Color.Red, 5), 0, 0, 300, 300);
            }
            pictureBox1.Image = b;
        }*/
    }
}
