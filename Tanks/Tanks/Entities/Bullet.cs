using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class Bullet : Entity
    {
        public int Speed;
        public Direction direction;

        public Bullet(int x, int y, Direction dir, int speed) : base(x, y)
        {
            Image = new Bitmap(Properties.Resources.Bullet);
			Speed = 2 * speed;
            direction = dir;
			Width = 6;
			Height = 6;
			X = x + 17;
			Y = y + 17;
			Name = "Bullet";
        }

		public void Move()
		{
			if (direction == Direction.LEFT)
			{
				X -= Speed;
			}
			if (direction == Direction.RIGHT)
			{
				X += Speed;
			}
			if (direction == Direction.UP)
			{
				Y -= Speed;
			}
			if (direction == Direction.DOWN)
			{
				Y += Speed;
			}
		}

		public void Render(Graphics g)
		{
			g.DrawImage(Image, X, Y);
		}
	}
}
