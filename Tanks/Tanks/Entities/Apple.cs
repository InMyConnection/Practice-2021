using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
	public class Apple : Entity
	{
		public Apple(Point pos) : base(pos)
		{
			Image = new Bitmap(@"Images\Apple.png");
			Name = "Apple";
		}

		public void Render(Graphics g)
		{
			g.DrawImage(Image, new Point(Position.X, Position.Y));
		}
	}
}
