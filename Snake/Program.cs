using System;
using static Snake.Game;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(FrameBuffer.WIDTH, FrameBuffer.HEIGHT);
            Console.SetBufferSize(FrameBuffer.WIDTH, FrameBuffer.HEIGHT);
            Console.Title = "Snake";
            Console.CursorVisible = false;

            FrameBuffer fb = new FrameBuffer();

            while(true)
            {
                Game g = new Game((uint)Environment.TickCount64);
                Result result = g.Run(ref fb);

                string message = result == Result.Win ? "You Win" : "You lose";

                int position = (FrameBuffer.WIDTH - message.Length) / 2;

                for(int i = 0; i < message.Length; i++)
                {
                    fb.SetPixel(position + i, FrameBuffer.HEIGHT / 2, message[i]);
                }

                fb.Render();

                Console.ReadKey(intercept: true);
            }
        }
    }
}
