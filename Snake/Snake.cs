﻿namespace Snake
{
    struct Snake
    {
        public const int MAXLENGTH = 30;
        private int _length;

        private unsafe fixed int _body[MAXLENGTH];

        private Direction _direction;
        private Direction _oldDirection;

        public Direction Course
        {
            set
            {
                if(_oldDirection != _direction)
                {
                    _oldDirection = _direction;
                }

                if(_direction - value != 2 && value - _direction != 2)
                {
                    _direction = value;
                }
            }
        }

        public unsafe Snake(byte x, byte y, Direction direction)
        {
            _body[0] = new Part(x, y, DirectionToChar(direction, direction)).Pack();
            _direction = direction;
            _oldDirection = direction;
            _length = 1;
        }

        public unsafe bool Update()
        {
            Part oldHead = Part.Unpack(_body[0]);
            Part newHead = new Part(
                (byte)(_direction switch
                {
                    Direction.Left => oldHead.X == 0 ? FrameBuffer.WIDTH - 1 : oldHead.X - 1,
                    Direction.Right => (oldHead.X + 1) % FrameBuffer.WIDTH, _ => oldHead.X
                }),
                (byte)(_direction switch
                {
                    Direction.Up => oldHead.Y == 0 ? FrameBuffer.HEIGHT - 1 : oldHead.Y - 1,
                    Direction.Down => (oldHead.Y + 1) % FrameBuffer.HEIGHT, _ => oldHead.Y
                }),
                DirectionToChar(_direction, _direction)
            );

            oldHead = new Part(oldHead.X, oldHead.Y, DirectionToChar(_oldDirection, _direction));

            bool result = true;

            for(int i = 0; i < _length - 1; i++)
            {
                Part current = Part.Unpack(_body[i]);
                if(current.X == newHead.X && current.Y == newHead.Y)
                {
                    result = false;
                }
            }

            _body[0] = oldHead.Pack();

            for(int i = _length - 1; i >= 0; i--)
            {
                _body[i + 1] = _body[i];
            }

            _body[0] = newHead.Pack();

            _oldDirection = _direction;

            return result;
        }

        public unsafe readonly void Draw(ref FrameBuffer fb)
        {
            for(int i = 0; i < _length; i++)
            {
                Part p = Part.Unpack(_body[i]);
                fb.SetPixel(p.X, p.Y, p.Character);
            }
        }

        public bool Extend()
        {
            if(_length < MAXLENGTH)
            {
                _length += 1;
                return true;
            }

            return false;
        }

        public unsafe readonly bool HitTest(int x, int y)
        {
            for (int i = 0; i < _length; i++)
            {
                Part current = Part.Unpack(_body[i]);
                if (current.X == x && current.Y == y)
                    return true;
            }

            return false;
        }

        private static char DirectionToChar(Direction oldDirection, Direction newDirection)
        {
            const string DirectionChangeToChar = "│┌?┐┘─┐??└│┘└?┌─";
            return DirectionChangeToChar[(int)oldDirection * 4 + (int)newDirection];
        }

        readonly struct Part
        {
            public readonly byte X, Y;
            public readonly char Character;

            public Part(byte x, byte y, char c)
            {
                X = x;
                Y = y;
                Character = c;
            }

            public int Pack() => X << 24 | Y << 16 | Character;
            public static Part Unpack(int packed) => new Part((byte)(packed >> 24), (byte)(packed >> 16), (char)packed);
        }

        public enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}
