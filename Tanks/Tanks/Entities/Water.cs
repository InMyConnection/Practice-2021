﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tanks.Entities
{
    public class Water : Entity
    {
        public Water(int x, int y) : base(x, y)
        {
            Width = 20;
            Height = 20;
            Name = "Water";
            Image = new Bitmap(Properties.Resources.Water);
        }

        public void Render(Graphics g)
        {
            g.DrawImage(Image, X, Y);
        }
    }
}