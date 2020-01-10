using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    unsafe struct FrameBuffer
    {
        public const int WIDTH = 40;
        public const int HEIGHT = 20;
        public const int AREA = WIDTH * HEIGHT;

        fixed char _chars[AREA];

        public void SetPixel(int x, int y, char character)
        {
            _chars[y * WIDTH + x] = character;
        }

        public void Clear()
        {
            for(int i = 0; i < AREA; i++)
            {
                _chars[i] = ' ';
            }
        }

        public readonly void Render()
        {
            Console.SetCursorPosition(0, 0);

            const ConsoleColor snakeColor = ConsoleColor.Green;

            Console.ForegroundColor = snakeColor;

            for(int i = 1; i <= AREA; i++)
            {
                char c = _chars[i - 1];

                if(c == '*' || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    Console.ForegroundColor = c == '*' ? ConsoleColor.Red : ConsoleColor.White;
                    Console.Write(c);
                    Console.ForegroundColor = snakeColor;
                } else
                {
                    Console.Write(c);
                }

                if(i % WIDTH == 0)
                {
                    Console.SetCursorPosition(0, i / WIDTH - 1);
                }
            }
        }
    }
}
