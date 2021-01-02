using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PxTetris.Core.GameScreenComponents
{
    public class Brick
    {
        private readonly IReadOnlyList<Square?[,]> brickKinds = new[]
        {
            new Square?[,] { { Square.Green, Square.Green, Square.Green, Square.Green } },
            new Square?[,] { { Square.Green }, { Square.Green }, { Square.Green }, { Square.Green } },
            new Square?[,] { { Square.Green, Square.Green }, { Square.Green, Square.Green } },
            new Square?[,] { { Square.Green, Square.Green }, { Square.Green, Square.Green } },
            new Square?[,] { { Square.Green, Square.Green, Square.Green }, { null, null, Square.Green } },
            new Square?[,] { { null, null, Square.Green }, { Square.Green, Square.Green, Square.Green } },
            new Square?[,] { { Square.Green, Square.Green, Square.Green }, { null, Square.Green, null } },
            new Square?[,] { { null, Square.Green, null }, { Square.Green, Square.Green, Square.Green } },
            new Square?[,] { { null, Square.Green, Square.Green }, { Square.Green, Square.Green, null } },
            new Square?[,] { { Square.Green, Square.Green, null }, { null, Square.Green, Square.Green } },
        };

        public Square?[,] Squares { get; private set; }
        public Point Position { get; private set; }
        private Point lastPosition;
        private static readonly Random random = new Random();

        public Brick()
        {
            Position = new Point(5, 0);

            int randomBrickType = random.Next(brickKinds.Count);
            Squares = brickKinds[randomBrickType];

            Square randomSquareType = (Square)random.Next(Enum.GetValues(typeof(Square)).Length);
            ChangeSquareType(randomSquareType);
        }

        private void ChangeSquareType(Square squareType)
        {
            for (int x = 0; x < Squares.GetLength(0); x++)
            {
                for (int y = 0; y < Squares.GetLength(1); y++)
                {
                    if (Squares[x, y].HasValue)
                    {
                        Squares[x, y] = squareType;
                    }
                }
            }
        }

        public void Rotate()
        {
            int squaresWidth = Squares.GetLength(0);
            int squaresHeight = Squares.GetLength(1);
            Square?[,] rotated = new Square?[squaresHeight, squaresWidth];

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
