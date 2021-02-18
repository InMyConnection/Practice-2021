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
    public partial class Game : Form
    {
        Kolobok player;
        Bitmap b;
        Resume resume;
        Random random = new Random();

        List<Tank> tanks;
        List<Apple> apples;
        BrickWall[] walls;
        List<Bullet> bulletsOfPlayer;
        List<Bullet> bulletsOfTanks;
        public List<Entity> entities;
        
        int score;
        int width;
        int height;
        int countOfTanks;
        int countOfApples;
        int speed;

        public Game()
        {
            InitializeComponent();
            countOfTanks = 5;
            countOfApples = 5;
            speed = 1;
        }

        public Game(int width, int height, int countOfTanks, int countOfApples, int speed)
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
            label1.Location = new Point(width + 40, 200);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = b;

            walls = new BrickWall[3];
            walls[0] = new BrickWall(50, 50, 270, 40);
            walls[1] = new BrickWall(50, 140, 40, 200);
            walls[2] = new BrickWall(140, 160, 190, 40);

            player = new Kolobok(340, 340, speed);

            tanks = new List<Tank>();
            for (int i = 0, x = 0; i < countOfTanks; i++, x += 45)
            {
                tanks.Add(new Tank(x, 0, speed));
            }

            apples = new List<Apple>();
            for (int i = 0; i < countOfApples; i++)
            {
                Point pos = GetRandomPosition();
                apples.Add(new Apple(pos.X, pos.Y));
            }

            bulletsOfPlayer = new List<Bullet>();
            bulletsOfTanks = new List<Bullet>();

            timerOfGame.Enabled = true;
            timerTankDirectionSwitchAndResumeUpdate.Enabled = true;
            score = 0;

            entities = new List<Entity>();
            entities.AddRange(walls);
            entities.Add(player);
            entities.AddRange(tanks);
            entities.AddRange(apples);

            resume = new Resume();
            resume.StartPosition = FormStartPosition.Manual;
            resume.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            resume.dataGridView1.DataSource = entities;
            resume.Show();
            this.Focus();
        }

        private void timerOfGame_Tick(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                for (int i = 0; i < walls.Length; i++)
                {
                    walls[i].Render(g);
                }

                Point posOfPlayer = new Point(player.X, player.Y);
                player.Move();
                if (CheckCollisionsWithBounds(player))
                {
                    player.X = posOfPlayer.X;
                    player.Y = posOfPlayer.Y;
                }
                player.Render(g);

                for (int i = 0; i < tanks.Count; i++)
                {
                    List<Point> posOfTank = tanks.Select(t => new Point(t.X, t.Y)).ToList();
                    tanks[i].Move();
                    int anotherTank = CheckCollisionWithAnotherTank(i);
                    if (anotherTank >= 0)
                    {
                        tanks[i].X = posOfTank[i].X;
                        tanks[i].Y = posOfTank[i].Y;
                        tanks[anotherTank].X = posOfTank[anotherTank].X;
                        tanks[anotherTank].Y = posOfTank[anotherTank].Y;
                    }
                    if (tanks[i].Fire())
                        bulletsOfTanks.Add(new Bullet(tanks[i].X, tanks[i].Y, tanks[i].direction, speed));
                    if (CheckCollisionsWithBounds(tanks[i]))
                    {
                        tanks[i].X = posOfTank[i].X;
                        tanks[i].Y = posOfTank[i].Y;
                    }

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

        private void timerTankDirectionSwitchAndResumeUpdate_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < tanks.Count; i++)
            {
                tanks[i].GetDirection();
            }

            resume.dataGridView1.DataSource = null;
            entities.Clear();
            entities.Add(player);
            entities.AddRange(walls);
            entities.AddRange(apples);
            entities.AddRange(tanks);
            entities.AddRange(bulletsOfTanks);
            entities.AddRange(bulletsOfPlayer);
            resume.dataGridView1.DataSource = entities;
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
                bulletsOfPlayer.Add(new Bullet(player.X, player.Y, player.direction, speed));
            }
        }

        public Point GetRandomPosition()
        {
            Point position;
            while (true)
            {
                position = new Point((int)random.Next(0, 340), (int)random.Next(0, 340));
                Entity entity = new Entity(position.X, position.Y);
                if (CheckCollisionsWithBounds(entity))
                    position = new Point((int)random.Next(0, 340), (int)random.Next(0, 300));
                else break;
            }
            return position;
        }

        private void CheckCollisions()
        {
            Rectangle playerRect = new Rectangle(player.X, player.Y , player.Width, player.Height);
            for (int i = 0; i < tanks.Count; i++)
            {
                Rectangle tankRect = new Rectangle(tanks[i].X, tanks[i].Y, tanks[i].Width, tanks[i].Height);
                if (Collide(playerRect, tankRect))
                {
                    timerOfGame.Enabled = false;
                    resume.Close();
                    MessageBox.Show("GAME OVER!", "Battle city");
                }

                for (int j = 0; j < bulletsOfPlayer.Count; j++)
                {
                    Rectangle bulletsOfPlayerRect = new Rectangle(bulletsOfPlayer[j].X, bulletsOfPlayer[j].Y, bulletsOfPlayer[j].Width, bulletsOfPlayer[j].Height);
                    if (Collide(tankRect, bulletsOfPlayerRect))
                    {
                        tanks.RemoveAt(i);
                        bulletsOfPlayer.RemoveAt(j);
                    } 
                }
            }
            for (int i = 0; i < apples.Count; i++)
            {
                Rectangle appleRect = new Rectangle(apples[i].X, apples[i].Y, apples[i].Width,apples[i].Height);
                if (Collide(playerRect, appleRect))
                {
                    apples.RemoveAt(i);
                    score++;
                    Point pos = GetRandomPosition();
                    apples.Add(new Apple(pos.X, pos.Y));
                }
            }

            for (int j = 0; j < bulletsOfTanks.Count; j++)
            {
                Rectangle bulletsOfTanksRect = new Rectangle(bulletsOfTanks[j].X, bulletsOfTanks[j].Y, bulletsOfTanks[j].Width, bulletsOfTanks[j].Height);
                if (Collide(playerRect, bulletsOfTanksRect))
                {
                    timerOfGame.Enabled = false;
                    resume.Close();
                    MessageBox.Show("GAME OVER!", "Battle city");
                }
            }
        }

        private bool CheckCollisionsWithBounds(Entity entity)
        {
            Rectangle entityRect = new Rectangle(entity.X, entity.Y, entity.Width, entity.Height);
            for (int j = 0; j < walls.Length; j++)
            {
                Rectangle wallRect = new Rectangle(walls[j].X, walls[j].Y, walls[j].Width, walls[j].Height);
                if (Collide(entityRect, wallRect))
                    return true;
            }
            if (entity.X < 0)
                return true;
            if (entity.Y < 0)
                return true;
            if (entity.X > pictureBox1.Width - entity.Width)
                return true;
            if (entity.Y > pictureBox1.Height - entity.Height)
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
                Rectangle tankRect = new Rectangle(tanks[i].X, tanks[i].Y, tanks[i].Width, tanks[i].Height);
                Rectangle tank2Rect = new Rectangle(tanks[k].X, tanks[k].Y, tanks[k].Width, tanks[k].Height);
                if (Collide(tankRect, tank2Rect))
                {
                    tanks[i].Rotate();
                    tanks[k].Rotate();
                    anotherTank = k;
                    break;
                }
            }
            return anotherTank;
        }

        private bool Collide(Rectangle rect1, Rectangle rect2)
        {
            return rect1.IntersectsWith(rect2);
        }
    }
}
