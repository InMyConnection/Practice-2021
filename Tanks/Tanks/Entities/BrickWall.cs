using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class BrickWall : Entity
    {
        public BrickWall(Point pos, int width, int height)
        {
            this.Position = pos;
            this.Width = width;
            this.Height = height;
        }

        public void Render(Graphics g)
        {
            g.FillRectangle(Brushes.Orange, Position.X, Position.Y, Width, Height);
        }

    }
}
