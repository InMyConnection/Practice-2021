using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class Kolobok : Entity
	{
		public int Speed;
		public Direction direction;
		Random random = new Random();
		public Kolobok(Point pos)
		{
			Speed = 1;
			direction = (Direction)random.Next(0, 4);
			this.Position = pos;
		}

		public void Move()
		{
			if (direction == Direction.LEFT)
			{
				Position.X -= Speed;
				Image = new Bitmap(@"Images\Kolobok\Left.png");
			}
			if (direction == Direction.RIGHT)
			{
				Position.X += Speed;
				Image = new Bitmap(@"Images\Kolobok\Right.png");
			}
			if (direction == Direction.UP)
			{
				Position.Y -= Speed;
				Image = new Bitmap(@"Images\Kolobok\Up.png");
			}
			if (direction == Direction.DOWN)
			{
				Position.Y += Speed;
				Image = new Bitmap(@"Images\Kolobok\Down.png");
			}
		}

		public void Render(Graphics g)
		{
			if (Image != null)
				g.DrawImage(Image, new Point(Position.X, Position.Y));
		}
	}
}
