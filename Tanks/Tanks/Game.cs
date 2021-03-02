using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tanks.Entities;
using System.IO;

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
        List<BrickWall> brickWalls;
        List<ConcreteWall> concreteWalls;
        List<Water> water;
        List<Bullet> bulletsOfPlayer;
        List<Bullet> bulletsOfTanks;
        List<Detonation> detonations;
        public List<Entity> entities;
        
        int score;
        int width;
        int height;
        int countOfTanks;
        int countOfApples;
        int speed;
        int countOfTanksToWin;

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
            countOfTanksToWin = 20;
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = b;
            DownloadLevel(5);

            player = new Kolobok(340, 480, speed);

            tanks = new List<Tank>();
            for (int i = 0, x = 0; i < countOfTanks; i++, x += 45)
            {
                tanks.Add(new Tank(x, 0, speed));
                countOfTanksToWin--;
            }

            apples = new List<Apple>();
            for (int i = 0; i < countOfApples; i++)
            {
                Point pos = GetRandomPosition();
                apples.Add(new Apple(pos.X, pos.Y));
            }

            bulletsOfPlayer = new List<Bullet>();
            bulletsOfTanks = new List<Bullet>();

            detonations = new List<Detonation>();

            timerOfGame.Enabled = true;
            timerResumeUpdate.Enabled = true;
            score = 0;

            entities = new List<Entity>();
            entities.Add(player);
            entities.AddRange(tanks);
            entities.AddRange(detonations);
            entities.AddRange(apples);
            entities.AddRange(water);
            entities.AddRange(brickWalls);
            entities.AddRange(concreteWalls);

            if (resume != null)
                resume.Close();
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
            CheckCollisions();
            using (Graphics g = Graphics.FromImage(b))
            {
                for (int i = 0; i < brickWalls.Count; i++)
                {
                    brickWalls[i].Render(g);
                }

                for (int i = 0; i < concreteWalls.Count; i++)
                {
                    concreteWalls[i].Render(g);
                }

                for (int i = 0; i < water.Count; i++)
                {
                    water[i].Render(g);
                }

                Point posOfPlayer = new Point(player.X, player.Y);
                player.Move();
                if (CheckCollisionsWithBounds(player, out int number, out int number2))
                {
                    player.X = posOfPlayer.X;
                    player.Y = posOfPlayer.Y;
                    player.GetDirection();
                }
                player.Render(g);

                for (int i = 0; i < tanks.Count; i++)
                {
                    List<Point> posOfTank = tanks.Select(t => new Point(t.X, t.Y)).ToList();
                    tanks[i].Move();
                    if (random.Next(0, 100) < 1)
                        tanks[i].GetDirection();
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
                    if (CheckCollisionsWithBounds(tanks[i], out int numb, out int numb2))
                    {
                        tanks[i].X = posOfTank[i].X;
                        tanks[i].Y = posOfTank[i].Y;
                        tanks[i].GetDirection();
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
                    if (CheckCollisionsWithBounds(bulletsOfPlayer[i], out int numberOfWall, out int numberOfWall2))
                    {
                        bulletsOfPlayer.RemoveAt(i);
                        if (numberOfWall >= 0)
                        {
                            detonations.Add(new Detonation(brickWalls[numberOfWall].X, brickWalls[numberOfWall].Y));
                            brickWalls.RemoveAt(numberOfWall);
                            if (numberOfWall2 > 0)
                                brickWalls.RemoveAt(numberOfWall2 - 1);
                        }
                    } 
                }

                for (int i = 0; i < bulletsOfTanks.Count; i++)
                {
                    bulletsOfTanks[i].Move();
                    bulletsOfTanks[i].Render(g);
                    if (CheckCollisionsWithBounds(bulletsOfTanks[i], out int numbOfWall, out int numbOfWall2))
                    {
                        bulletsOfTanks.RemoveAt(i);
                        if (numbOfWall >= 0)
                        {
                            detonations.Add(new Detonation(brickWalls[numbOfWall].X, brickWalls[numbOfWall].Y));
                            brickWalls.RemoveAt(numbOfWall);
                            if (numbOfWall2 > 0)
                                brickWalls.RemoveAt(numbOfWall2 - 1);
                        }
                    }
                }

                for (int i = 0; i < detonations.Count; i++)
                {
                    detonations[i].Render(g);
                }
            }
            pictureBox1.Image = b;
            txtScore.Text = "Apples: " + score;
            detonations.Clear();
            if (timerOfGame.Enabled == false)
                MessageBox.Show("GAME OVER!", "Battle city");
            if (tanks.Count == 0)
            {
                timerOfGame.Enabled = false;
                MessageBox.Show("YOU WIN!", "Battle city");
            }
        }

        public void DownloadLevel(int currentLevel)
        {
            string level = "_" + currentLevel.ToString();
            string map = Properties.Resources.ResourceManager.GetObject(level).ToString();

            string[] linesTileMap = map.Split('\n');

            concreteWalls = new List<ConcreteWall>();
            brickWalls = new List<BrickWall>();
            water = new List<Water>();

            int x = 0, y = 0;
            foreach (string line in linesTileMap)
            {
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case '#':
                            brickWalls.Add(new BrickWall(x, y));
                            break;
                        case '@':
                            concreteWalls.Add(new ConcreteWall(x, y));
                            break;
                        case '~':
                            water.Add(new Water(x, y));
                            break;
                    }
                    x += 20;
                }
                x = 0;
                y += 20;
            }
        }

        private void timerResumeUpdate_Tick(object sender, EventArgs e)
        {
            resume.dataGridView1.DataSource = null;
            entities.Clear();
            entities.Add(player);
            entities.AddRange(tanks);
            entities.AddRange(bulletsOfTanks);
            entities.AddRange(bulletsOfPlayer);
            entities.AddRange(detonations);
            entities.AddRange(apples);
            entities.AddRange(water);
            entities.AddRange(brickWalls);
            entities.AddRange(concreteWalls);
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
            if (e.KeyCode == Keys.Space)
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
                if (CheckCollisionsWithBounds(entity, out int numb, out int numb2))
                    position = new Point((int)random.Next(0, 340), (int)random.Next(0, 300));
                else break;
            }
            return position;
        }

        public int GetPositionOfTank()
        {
            int x = 0;
            for (int i = 0; i < tanks.Count; i++)
            {
                Rectangle tankRect = new Rectangle(tanks[i].X, tanks[i].Y, tanks[i].Width, tanks[i].Height);
                Rectangle newTankRect = new Rectangle(x, 0, tanks[i].Width, tanks[i].Height);
                if (Collide(tankRect, newTankRect))
                {
                    x += 45;
                    i = 0;
                }
            }
            return x;
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
                }

                for (int j = 0; j < bulletsOfPlayer.Count; j++)
                {
                    Rectangle bulletsOfPlayerRect = new Rectangle(bulletsOfPlayer[j].X, bulletsOfPlayer[j].Y, bulletsOfPlayer[j].Width, bulletsOfPlayer[j].Height);
                    if (Collide(tankRect, bulletsOfPlayerRect))
                    {
                        detonations.Add(new Detonation(tanks[i].X, tanks[i].Y));
                        tanks.RemoveAt(i);
                        bulletsOfPlayer.RemoveAt(j);
                        if (countOfTanksToWin > 0)
                        {
                            tanks.Add(new Tank(GetPositionOfTank(), 0, speed));
                            countOfTanksToWin--;
                        }
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
                    detonations.Add(new Detonation(player.X, player.Y));
                    resume.Close();
                    timerOfGame.Enabled = false;
                }
            }
        }

        private bool CheckCollisionsWithBounds(Entity entity, out int numberOfWall, out int numberOfWall2)
        {
            numberOfWall = -1;
            numberOfWall2 = -1;
            int j = 0;
            bool collision = false;

            Rectangle entityRect = new Rectangle(entity.X, entity.Y, entity.Width, entity.Height);

            for (int i = 0; i < concreteWalls.Count; i++)
            {
                Rectangle wallRect = new Rectangle(concreteWalls[i].X, concreteWalls[i].Y, concreteWalls[i].Width, concreteWalls[i].Height);
                if (Collide(entityRect, wallRect))
                {
                    collision = true;
                    break;
                }
            }

            for (; j < brickWalls.Count; j++)
            {
                Rectangle wallRect = new Rectangle(brickWalls[j].X, brickWalls[j].Y, brickWalls[j].Width, brickWalls[j].Height);
                if (Collide(entityRect, wallRect))
                {
                    numberOfWall = j;
                    collision = true;
                    break;
                }
            }

            for (j++; j < brickWalls.Count; j++)
            {
                Rectangle wallRect = new Rectangle(brickWalls[j].X, brickWalls[j].Y, brickWalls[j].Width, brickWalls[j].Height);
                if (Collide(entityRect, wallRect))
                {
                    numberOfWall2 = j;
                }
            }

            if (!(entity is Bullet))
            {
                for (int i = 0; i < water.Count; i++)
                {
                    Rectangle waterRect = new Rectangle(water[i].X, water[i].Y, water[i].Width, water[i].Height);
                    if (Collide(entityRect, waterRect))
                    {
                        collision = true;
                        break;
                    }
                }
            }

            if (entity.X < 0)
                collision = true;
            if (entity.Y < 0)
                collision = true;
            if (entity.X > pictureBox1.Width - entity.Width)
                collision = true;
            if (entity.Y > pictureBox1.Height - entity.Height)
                collision = true;
            return collision;
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

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                this.Focus();
        }
    }
}
