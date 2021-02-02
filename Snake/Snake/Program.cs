using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetBufferSize(80, 25);

            Walls walls = new Walls(80, 24);
            walls.Draw();

            Point p = new Point(4, 5, '*');
            Snake snake = new Snake(p, 4, Direction.RIGHT);
            snake.Draw();
            snake.WriteCountOfFood();

            FoodCreator foodCreator = new FoodCreator(80, 24, '$');
            Point food = foodCreator.CreateFood();
            food.Draw();


            while (true)
            {
                if (walls.IsHit(snake) || snake.IsHitTail())
                {
                    break;
                }
                if (snake.Eat(food))
                {
                    food = foodCreator.CreateFood();
                    food.Draw();
                    snake.WriteCountOfFood();
                }
                else
                    snake.Move();

                Thread.Sleep(100);

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    snake.HandleKey(key.Key);
                }
                Thread.Sleep(100);
                snake.Move();
            }
            WriteGameOver();
            Console.ReadLine();
        }
        static void WriteGameOver()
        {
            Console.SetCursorPosition(30, 10);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Game over!");
        }
    }
}
