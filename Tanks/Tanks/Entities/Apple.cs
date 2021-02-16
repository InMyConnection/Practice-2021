using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
	public class Apple : Entity
	{
		Random random = new Random();
		public Apple()
		{
			Image = new Bitmap(@"Images\Apple.png");
			this.Position = GetRandomPosition();
		}

		public void Render(Graphics g)
		{
			g.DrawImage(Image, new Point(Position.X, Position.Y));
		}

		public Point GetRandomPosition()
		{
			Point position = new Point((int)random.Next(0, 340), (int)random.Next(0, 200));
			return position;
		}
	}
}
