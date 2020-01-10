using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Snake
{
    struct Game
    {
        public enum Result
        {
            Win,
            Loss
        }

        private Random _random;

        public Game(uint randomSeed)
        {
            _random = new Random(randomSeed);
        }

        public Result Run(ref FrameBuffer fb)
        {
            Snake s = new Snake(
                (byte)(_random.Next() % FrameBuffer.WIDTH),
                (byte)(_random.Next() % FrameBuffer.HEIGHT),
                (Snake.Direction)(_random.Next() % 4)
            );

            MakeFood(s, out byte foodX, out byte foodY);
            
            long gameTime = Environment.TickCount64;

            while(true)
            {
                fb.Clear();

                if(!s.Update()) {
                    s.Draw(ref fb);
                    return Result.Loss;
                }

                s.Draw(ref fb);

                if(Console.KeyAvailable)
                {
                    ConsoleKeyInfo ki = Console.ReadKey(intercept: true);
                    switch(ki.Key)
                    {
                        case ConsoleKey.UpArrow:
                            s.Course = Snake.Direction.Up;
                            break;
                        case ConsoleKey.DownArrow:
                            s.Course = Snake.Direction.Down;
                            break;
                        case ConsoleKey.LeftArrow:
                            s.Course = Snake.Direction.Left;
                            break;
                        case ConsoleKey.RightArrow:
                            s.Course = Snake.Direction.Right;
                            break;
                    }
                }

                if(s.HitTest(foodX, foodY))
                {
                    if(s.Extend())
                    {
                        MakeFood(s, out foodX, out foodY);
                    } else
                    {
                        return Result.Win;
                    }
                }

                fb.SetPixel(foodX, foodY, '*');
                fb.Render();

                gameTime += 100;

                long delay = gameTime - Environment.TickCount64;
                if(delay >= 0)
                {
                    Thread.Sleep((int)delay);
                } else
                {
                    gameTime = Environment.TickCount64;
                }
            }
        }

        void MakeFood(in Snake snake, out byte foodX, out byte foodY)
        {
            do
            {
                foodX = (byte)(_random.Next() % FrameBuffer.WIDTH);
                foodY = (byte)(_random.Next() % FrameBuffer.HEIGHT);
            } while (snake.HitTest(foodX, foodY));
        }
    }
}
