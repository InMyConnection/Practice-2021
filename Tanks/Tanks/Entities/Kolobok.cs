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
		Bitmap[] images;

		public Kolobok(int x, int y, int speed) : base(x, y)
		{
			Speed = speed;
			Name = "Player";
			GetDirection();
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

		public void GetImage()
		{
			images = new Bitmap[4];
			images[0] = Properties.Resources.pl_Left;
			images[1] = Properties.Resources.pl_Right;
			images[2] = Properties.Resources.pl_Up;
			images[3] = Properties.Resources.pl_Down;
		}

		public void Render(Graphics g)
		{
			g.DrawImage(Image, X, Y);
		}
	}
}
