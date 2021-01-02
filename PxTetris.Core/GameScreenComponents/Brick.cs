using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PxTetris.Core.GameScreenComponents
{
    public class Brick
    {
        private static readonly IReadOnlyList<bool[,]> brickKinds = new[]
        {
            new bool[,] { { true, true, true, true } },
            new bool[,] { { true }, { true }, { true }, { true } },
            new bool[,] { { true, true }, { true, true } },
            new bool[,] { { true, true }, { true, true } },
            new bool[,] { { true, true, true }, { false, false, true } },
            new bool[,] { { false, false, true }, { true, true, true } },
            new bool[,] { { true, true, true }, { false, true, false } },
            new bool[,] { { false, true, false }, { true, true, true } },
            new bool[,] { { false, true, true }, { true, true, false } },
            new bool[,] { { true, true, false }, { false, true, true } },
        };

        public Square[,] Squares { get; private set; }
        public Point Position { get; private set; }
        private Point lastPosition;
        private static readonly Random random = new Random();

        public Brick()
        {
            Position = new Point(5, 0);

            var randomBrickKind = brickKinds[random.Next(brickKinds.Count)];
            Square randomSquare = Square.GetRandomSquare(random);
            Squares = GetSquares(randomBrickKind, randomSquare);
        }

        private static Square[,] GetSquares(bool[,] brickKind, Square squareType)
        {
            var result = new Square[brickKind.GetLength(0), brickKind.GetLength(1)];

            for (int x = 0; x < brickKind.GetLength(0); x++)
            {
                for (int y = 0; y < brickKind.GetLength(1); y++)
                {
                    if (brickKind[x, y] != false)
                    {
                        result[x, y] = squareType;
                    }
                }
            }

            return result;
        }

        public void Rotate()
        {
            int squaresWidth = Squares.GetLength(0);
            int squaresHeight = Squares.GetLength(1);
            Square[,] rotated = new Square[squaresHeight, squaresWidth];

            for (int x = 0; x < squaresHeight; x++)
            {
                for (int y = 0; y < squaresWidth; y++)
                {
                    rotated[x, y] = Squares[y, squaresHeight - 1 - x];
                }
            }

            Squares = rotated;
        }

        public void UndoRotation()
        {
            Rotate();
            Rotate();
            Rotate();
        }

        public void MoveDown()
        {
            lastPosition = Position;
            Position = new Point(Position.X, Position.Y + 1);
        }

        public void MoveLeft()
        {
            lastPosition = Position;
            Position = new Point(Position.X - 1, Position.Y);
        }

        public void MoveRight()
        {
            lastPosition = Position;
            Position = new Point(Position.X + 1, Position.Y);
        }

        public void UndoMove()
        {
            Position = lastPosition;
        }
    }
}
