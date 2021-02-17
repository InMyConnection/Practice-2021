using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class BrickWall : Entity
    {
        public BrickWall(Point pos, Size dim) : base(pos)
        {
            Dimension = dim;
            Name = "Wall";
        }

        public void Render(Graphics g)
        {
            g.FillRectangle(Brushes.Orange, Position.X, Position.Y, Dimension.Width, Dimension.Height);
        }

    }
}
