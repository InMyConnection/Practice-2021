using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class BrickWall : Entity
    {
        public BrickWall(int x, int y, int width, int height) : base(x, y)
        {
            Width = width;
            Height = height;
            Name = "Wall";
        }

        public void Render(Graphics g)
        {
            g.FillRectangle(Brushes.Orange, X, Y, Width, Height);
        }

    }
}
