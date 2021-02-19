using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
	public class Detonation : Entity
	{
		public Detonation(int x, int y) : base(x, y)
		{
			Image = new Bitmap(@"Images\Detonation.png");
			Name = "Detonation";
		}

		public void Render(Graphics g)
		{
			g.DrawImage(Image, X, Y);
		}
	}
}
