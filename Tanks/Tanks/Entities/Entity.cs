using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
	public class Entity
	{
		public int Width = 40;
		public int Height = 40;
		public Bitmap Image;
		public string Name { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public Entity(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}
