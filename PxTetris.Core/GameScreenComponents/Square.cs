using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PxTetris.Core.GameScreenComponents
{
    public class Square
    {
        public const int Size = 20;

        private static readonly Square BlueSquare = new Square();
        private static readonly Square GreenSquare = new Square();
        private static readonly Square RedSquare = new Square();
        private static readonly Square YellowSquare = new Square();
        private static readonly Square PurpleSquare = new Square();

        private Square()
        {
        }

        public static Square GetRandomSquare(Random random)
        {
            var squareTypes = new[] { BlueSquare, GreenSquare, RedSquare, YellowSquare, PurpleSquare };
            return squareTypes[random.Next(squareTypes.Length)];
        }

        public void Draw(SpriteBatch spriteBatch, Textures textures, int x, int y, int xOffset, int yOffset, float scale = 1)
        {
            Texture2D texture = GetTexture(textures);
            float scaledSize = Size * scale;
            Rectangle destination = new Rectangle(xOffset + (int)(x * scaledSize), yOffset + (int)(y * scaledSize), (int)scaledSize, (int)scaledSize);
            spriteBatch.Draw(texture, destination, Color.White);
        }

        private Texture2D GetTexture(Textures textures)
        {
            if (this == BlueSquare)
            {
                return textures.BrickBlue;
            }
            if (this == GreenSquare)
            {
                return textures.BrickGreen;
            }
            if (this == RedSquare)
            {
                return textures.BrickRed;
            }
            if (this == YellowSquare)
            {
                return textures.BrickYellow;
            }
            if (this == PurpleSquare)
            {
                return textures.BrickPurple;
            }
            throw new NotSupportedException("Not supported square type.");
        }
    }
}
