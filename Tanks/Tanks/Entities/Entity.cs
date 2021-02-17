using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
	public class Entity
	{
		public Size Dimension = new Size(40, 40);
		public Point Position;
		public Bitmap Image;
		public string Name { get; set; }
		public int X { get { return X; } set { X = Position.X; } }
		public int Y { get { return Y; } set { Y = Position.Y; } }

		public Entity(Point pos)
		{
			Position = pos;
		}
	}
}
