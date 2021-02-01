using System;
using System.Collections.Generic;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Point p1 = new Point(1, 2, '*');
            p1.Draw();

            Point p2 = new Point(4, 5, '#');
            p2.Draw();

            HorizontalLine hLine = new HorizontalLine(5, 10, 8, '+');
            hLine.Draw();

            VerticalLine vLine = new VerticalLine(14, 4, 11, '&');
            vLine.Draw();

            Console.ReadKey();
        }
    }
}
