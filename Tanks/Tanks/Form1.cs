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
        List<Tank> tanks;
        List<Apple> apples;
        List<Bullet> bulletsOfPlayer;
        List<Bullet> bulletsOfTanks;
        public static List<Entity> entities;
        BrickWall[] walls;
        Bitmap b;
        Random random = new Random();
        int score;
        int width;
        int height;
        int countOfTanks;
        int countOfApples;
        int speed;
        Form2 resume;

        public Form1()
        {
            InitializeComponent();
            countOfTanks = 5;
            countOfApples = 5;
            speed = 1;

        }

        public Form1(int width, int height, int countOfTanks, int countOfApples, int speed)
        {
            InitializeComponent();
            this.width = width;
            this.height = height;
            this.countOfTanks = countOfTanks;
            this.countOfApples = countOfApples;
            this.speed = speed;
            pictureBox1.Size = new Size(width, height);
            this.Size = new Size(width + 181, height + 35);
            txtScore.Location = new Point(width, 9);
            buttonStart.Location = new Point(width + 40, 125);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = b;

            walls = new BrickWall[3];
            walls[0] = new BrickWall(new Point(50, 50), new Size(270, 40));
            walls[1] = new BrickWall(new Point(50, 140), new Size(40, 200));
            walls[2] = new BrickWall(new Point(140, 160), new Size(190, 40));

            player = new Kolobok(new Point(340, 340), speed);

            tanks = new List<Tank>();
            for (int i = 0, x = 0; i < countOfTanks; i++, x += 45)
            {
                tanks.Add(new Tank(new Point(x, 0), speed));
            }

            apples = new List<Apple>();
            for (int i = 0; i < countOfApples; i++)
            {
                apples.Add(new Apple(GetRandomPosition()));
            }

            bulletsOfPlayer = new List<Bullet>();
            bulletsOfTanks = new List<Bullet>();
            entities = new List<Entity>();
            entities.Add(player);
            entities.AddRange(walls);
            entities.AddRange(tanks);
            entities.AddRange(apples);

            primaryTimer.Enabled = true;
            tankDirectionSwitch.Enabled = true;
            score = 0;
            resume = new Form2();
            resume.Show();
            this.Focus();
        }

        private void primaryTimer_Tick(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                for (int i = 0; i < walls.Length; i++)
                {
                    walls[i].Render(g);
                }

                Point posOfPlayer = player.Position;
                player.Move();
                if (CheckCollisionsWithBounds(player))
                    player.Position = posOfPlayer;
                player.Render(g);

                for (int i = 0; i < tanks.Count; i++)
                {
                    List<Point> posOfTank = tanks.Select(t => t.Position).ToList();
                    tanks[i].Move();
                    int anotherTank = CheckCollisionWithAnotherTank(i);
                    if (anotherTank >= 0)
                    {
                        tanks[i].Position = posOfTank[i];
                        tanks[anotherTank].Position = posOfTank[anotherTank];
                    }
                    if (tanks[i].Fire())
                        bulletsOfTanks.Add(new Bullet(tanks[i].Position, tanks[i].direction, speed));
                    if (CheckCollisionsWithBounds(tanks[i]))
                        tanks[i].Position = posOfTank[i];
                    tanks[i].Render(g);
                }

                for (int i = 0; i < apples.Count; i++)
                {
                    apples[i].Render(g);
                }

                for (int i = 0; i < bulletsOfPlayer.Count; i++)
                {
                    bulletsOfPlayer[i].Move();
                    bulletsOfPlayer[i].Render(g);
                    if (CheckCollisionsWithBounds(bulletsOfPlayer[i]))
                        bulletsOfPlayer.RemoveAt(i);
                }

                for (int i = 0; i < bulletsOfTanks.Count; i++)
                {
                    bulletsOfTanks[i].Move();
                    bulletsOfTanks[i].Render(g);
                    if (CheckCollisionsWithBounds(bulletsOfTanks[i]))
                        bulletsOfTanks.RemoveAt(i);
                }
            }
            pictureBox1.Image = b;
            txtScore.Text = "Apples: " + score;
            CheckCollisions();
        }

        private void tankDirectionSwitch_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < tanks.Count; i++)
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
            if (e.KeyCode == Keys.B)
            {
                bulletsOfPlayer.Add(new Bullet(player.Position, player.direction, speed));
            }
        }

        public Point GetRandomPosition()
        {
            Point position;
            while (true)
            {
                position = new Point((int)random.Next(0, 340), (int)random.Next(0, 340));
                Entity entity = new Entity(position);
                if (CheckCollisionsWithBounds(entity))
                    position = new Point((int)random.Next(0, 340), (int)random.Next(0, 300));
                else break;
            }
            return position;
        }

        private void CheckCollisions()
        {
            Rectangle playerRect = new Rectangle(player.Position, player.Dimension);
            for (int i = 0; i < tanks.Count; i++)
            {
                Rectangle tankRect = new Rectangle(tanks[i].Position, tanks[i].Dimension);
                if (Collide(playerRect, tankRect))
                {
                    primaryTimer.Enabled = false;
                    MessageBox.Show("GAME OVER!", "Battle city");
                }

                for (int j = 0; j < bulletsOfPlayer.Count; j++)
                {
                    Rectangle bulletsOfPlayerRect = new Rectangle(bulletsOfPlayer[j].Position, bulletsOfPlayer[j].Dimension);
                    if (Collide(tankRect, bulletsOfPlayerRect))
                    {
                        tanks.RemoveAt(i);
                        bulletsOfPlayer.RemoveAt(j);
                    } 
                }
            }
            for (int i = 0; i < apples.Count; i++)
            {
                Rectangle appleRect = new Rectangle(apples[i].Position, apples[i].Dimension);
                if (Collide(playerRect, appleRect))
                {
                    apples.RemoveAt(i);
                    score++;
                    apples.Add(new Apple(GetRandomPosition()));
                }
            }

            for (int j = 0; j < bulletsOfTanks.Count; j++)
            {
                Rectangle bulletsOfTanksRect = new Rectangle(bulletsOfTanks[j].Position, bulletsOfTanks[j].Dimension);
                if (Collide(playerRect, bulletsOfTanksRect))
                {
                    primaryTimer.Enabled = false;
                    MessageBox.Show("GAME OVER!", "Battle city");
                }
            }
        }

        private bool CheckCollisionsWithBounds(Entity entity)
        {
            Rectangle entityRect = new Rectangle(entity.Position, entity.Dimension);
            for (int j = 0; j < walls.Length; j++)
            {
                Rectangle wallRect = new Rectangle(walls[j].Position, walls[j].Dimension);
                if (Collide(entityRect, wallRect))
                    return true;
            }
            if (entity.Position.X < 0)
                return true;
            if (entity.Position.Y < 0)
                return true;
            if (entity.Position.X > pictureBox1.Width - entity.Dimension.Width)
                return true;
            if (entity.Position.Y > pictureBox1.Height - entity.Dimension.Height)
                return true;
            return false;
        }

        private int CheckCollisionWithAnotherTank(int i)
        {
            int anotherTank = -1;
            for (int k = 0; k < tanks.Count; k++)
            {
                if (k == i)
                    continue;
                Rectangle tankRect = new Rectangle(tanks[i].Position, tanks[i].Dimension);
                Rectangle tank2Rect = new Rectangle(tanks[k].Position, tanks[k].Dimension);
                if (Collide(tankRect, tank2Rect))
                {
                    tanks[i].Rotate();
                    tanks[k].Rotate();
                    anotherTank = k;
                }
            }
            return anotherTank;
        }

        private bool Collide(Rectangle rect1, Rectangle rect2)
        {
            return rect1.IntersectsWith(rect2);
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
