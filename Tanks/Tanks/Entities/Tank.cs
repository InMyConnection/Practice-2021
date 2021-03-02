using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class Tank : Entity
    {
		public int Speed;
		public Direction direction;
		Random random = new Random();
		Bitmap[] images;

		public Tank(int x, int y, int speed) : base(x, y)
		{
			Speed = speed;
			GetDirection();
			Name = "Tank";
			GetImage();
		}

		public void Move()
		{
			if (direction == Direction.LEFT)
			{
				X -= Speed;
				Image = images[0];
			}
			if (direction == Direction.RIGHT)
			{
				X += Speed;
				Image = images[1];
			}
			if (direction == Direction.UP)
			{
				Y -= Speed;
				Image = images[2];
			}
			if (direction == Direction.DOWN)
			{
				Y += Speed;
				Image = images[3];
			}
		}

		public void GetDirection()
        {
			direction = (Direction)random.Next(0, 4);
		}

		public bool Fire()
        {
			if (random.Next(0, 100) > 98)
				return true;
			return false;
        }

		public void GetImage()
        {
			images = new Bitmap[4];
			images[0] = Properties.Resources.Left;
			images[1] = Properties.Resources.Right;
			images[2] = Properties.Resources.Up;
			images[3] = Properties.Resources.Down;
		}

		public void Render(Graphics g)
		{
			g.DrawImage(Image, X, Y);
		}

		public void Rotate()
		{
			switch (direction)
			{
				case Direction.LEFT:
					direction = Direction.RIGHT;
					break;

				case Direction.RIGHT:
					direction = Direction.LEFT;
					break;

				case Direction.UP:
					direction = Direction.DOWN;
					break;

				case Direction.DOWN:
					direction = Direction.UP;
					break;
			}
		}
	}
}
