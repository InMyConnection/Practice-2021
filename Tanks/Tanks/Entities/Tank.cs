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

		public Tank(int x, int y, int speed) : base(x, y)
		{
			Speed = speed;
			GetDirection();
			Name = "Tank";
		}

		public void Move()
		{
			if (direction == Direction.LEFT)
			{
				X -= Speed;
				Image = new Bitmap(@"Images\Tank\Left.png");
			}
			if (direction == Direction.RIGHT)
			{
				X += Speed;
				Image = new Bitmap(@"Images\Tank\Right.png");
			}
			if (direction == Direction.UP)
			{
				Y -= Speed;
				Image = new Bitmap(@"Images\Tank\Up.png");
			}
			if (direction == Direction.DOWN)
			{
				Y += Speed;
				Image = new Bitmap(@"Images\Tank\Down.png");
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
