using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    class ConcreteWall : Entity
    {
        public ConcreteWall(int x, int y) : base(x, y)
        {
            Width = 20;
            Height = 20;
            Image = new Bitmap(Properties.Resources.ConcreteWall);
            Name = "Concrete Wall";
        }

        public void Render(Graphics g)
        {
            g.DrawImage(Image, X, Y);
        }
    }
}
