using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class Water : Entity
    {
        public Water(int x, int y, int width, int height) : base(x, y)
        {
            Width = width;
            Height = height;
            Name = "Water";
        }

        public void Render(Graphics g)
        {
            g.FillRectangle(Brushes.Blue, X, Y, Width, Height);
        }
    }
}
