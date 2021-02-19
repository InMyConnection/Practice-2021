using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class BrickWall : Entity
    {
        public BrickWall(int x, int y) : base(x, y)
        {
            Width = 20;
            Height = 20;
            Image = new Bitmap(@"Images\BrickWall.png");
            Name = "Wall";
        }

        public void Render(Graphics g)
        {
            g.DrawImage(Image, X, Y);
        }
    }
}
