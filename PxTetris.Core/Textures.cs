using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PxTetris.Core
{
    public class Textures
    {
        public Texture2D BrickBlue { get; private set; }
        public Texture2D BrickGreen { get; private set; }
        public Texture2D BrickPurple { get; private set; }
        public Texture2D BrickRed { get; private set; }
        public Texture2D BrickYellow { get; private set; }
        public Texture2D WhiteRectangle { get; private set; }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            BrickBlue = content.Load<Texture2D>(nameof(BrickBlue));
            BrickGreen = content.Load<Texture2D>(nameof(BrickGreen));
            BrickPurple = content.Load<Texture2D>(nameof(BrickPurple));
            BrickRed = content.Load<Texture2D>(nameof(BrickRed));
            BrickYellow = content.Load<Texture2D>(nameof(BrickYellow));

            WhiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            WhiteRectangle.SetData(new[] { Color.White });
        }
    }
}
