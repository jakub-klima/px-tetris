using Microsoft.Xna.Framework.Graphics;

namespace PxTetris.Core.GameScreenComponents
{
    public class SquareGrid
    {
        public Square[,] Items { get; } = new Square[13, 18];

        public bool AnyRowFull()
        {
            return GetFullRow().HasValue;
        }

        public void ClearRow()
        {
            int fullRow = GetFullRow().Value;
            for (int y = fullRow - 1; y >= 0; y--)
            {
                for (int x = 0; x < Items.GetLength(0); x++)
                {
                    Items[x, y + 1] = Items[x, y];
                }
            }
        }

        private int? GetFullRow()
        {
            for (int y = Items.GetLength(1) - 1; y >= 0; y--)
            {
                bool isRowFull = true;

                for (int x = 0; x < Items.GetLength(0); x++)
                {
                    if (Items[x, y] == null)
                    {
                        isRowFull = false;
                        break;
                    }
                }

                if (isRowFull)
                {
                    return y;
                }
            }

            return null;
        }

        public void AddBrick(Brick brick)
        {
            for (int x = 0; x < brick.Squares.GetLength(0); x++)
            {
                for (int y = 0; y < brick.Squares.GetLength(1); y++)
                {
                    if (brick.Squares[x, y] != null)
                    {
                        Items[brick.Position.X + x, brick.Position.Y + y] = brick.Squares[x, y];
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Textures textures, int yOffset)
        {
            for (int x = 0; x < Items.GetLength(0); x++)
            {
                for (int y = 0; y < Items.GetLength(1); y++)
                {
                    Items[x, y]?.Draw(spriteBatch, textures, x, y, 0, yOffset);
                }
            }
        }
    }
}
