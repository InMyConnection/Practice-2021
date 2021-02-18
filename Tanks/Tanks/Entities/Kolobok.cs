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
		public Kolobok(int x, int y, int speed) : base(x, y)
		{
			Speed = speed;
			direction = (Direction)random.Next(0, 4);
			Name = "Player";
		}

		public void Move()
		{
			if (direction == Direction.LEFT)
			{
				X -= Speed;
				Image = new Bitmap(@"Images\Kolobok\Left.png");
			}
			if (direction == Direction.RIGHT)
			{
				X += Speed;
				Image = new Bitmap(@"Images\Kolobok\Right.png");
			}
			if (direction == Direction.UP)
			{
				Y -= Speed;
				Image = new Bitmap(@"Images\Kolobok\Up.png");
			}
			if (direction == Direction.DOWN)
			{
				Y += Speed;
				Image = new Bitmap(@"Images\Kolobok\Down.png");
			}
		}

		public void Render(Graphics g)
		{
			g.DrawImage(Image, X, Y);
		}
	}
}
