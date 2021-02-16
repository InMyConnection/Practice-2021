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

		public Tank()
		{
			Speed = 1;
			GetDirection();
			this.Position = GetRandomPosition();
		}

		public void Move()
		{
			if (direction == Direction.LEFT)
			{
				Position.X -= Speed;
				Image = new Bitmap(@"Images\Tank\Left.png");
			}
			if (direction == Direction.RIGHT)
			{
				Position.X += Speed;
				Image = new Bitmap(@"Images\Tank\Right.png");
			}
			if (direction == Direction.UP)
			{
				Position.Y -= Speed;
				Image = new Bitmap(@"Images\Tank\Up.png");
			}
			if (direction == Direction.DOWN)
			{
				Position.Y += Speed;
				Image = new Bitmap(@"Images\Tank\Down.png");
			}
		}

		public void GetDirection()
        {
			direction = (Direction)random.Next(0, 4);
		}

		public Point GetRandomPosition()
        {
			Point position = new Point((int)random.Next(0, 340), (int)random.Next(0, 200));
			return position;
        }

		public void Render(Graphics g)
		{
			g.DrawImage(Image, new Point(Position.X, Position.Y));
		}

		public void Rotate()
		{
			if (direction == Direction.LEFT)
				direction = Direction.RIGHT;
			
			if (direction == Direction.RIGHT)
				direction = Direction.LEFT;
			
			if (direction == Direction.UP)
				direction = Direction.DOWN;
			
			if (direction == Direction.DOWN)
				direction = Direction.UP;
		}
	}
}
