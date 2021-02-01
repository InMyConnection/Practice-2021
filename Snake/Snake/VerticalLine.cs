using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class VerticalLine
    {
        List<Point> pList;

        public VerticalLine(int x, int yDown, int yUp, char sym)
        {
            pList = new List<Point>();
            for (int y = yDown; y <= yUp; y++)
            {
                Point p = new Point(x, y, sym);
                pList.Add(p);
            }
        }

        public void Draw()
        {
            foreach (Point p in pList)
            {
                p.Draw();
            }
        }
    }
}
