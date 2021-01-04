using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PxTetris.Core;

namespace PxTetris.Android
{
    public class AndroidTetrisGame : TetrisGame
    {
        private int screenWidth;
        private int screenHeight;

        protected override void Initialize()
        {
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = (int)(screenWidth * 16.0 / 9); // CurrentDisplayMode.Height does not have correct value in Initialize(). Also we want to force 16:9 for full-view screens.

            scale = screenWidth / 260f;
            keyboard = new AndroidGameKeyboard(screenWidth, () => activeScreen);

            base.Initialize();
        }

        protected override void AjustGraphics(GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = true;
        }

        protected override void DrawAdHoc(SpriteBatch spriteBatch, Textures textures, SpriteFont font)
        {
            DrawControlsHint(spriteBatch, textures.WhiteRectangle, font);
        }

        private void DrawControlsHint(SpriteBatch spriteBatch, Texture2D whiteRectangleTexture, SpriteFont font)
        {
            // TODO Sdilet souradnice / delky s Core a AndroidGameKeyboard
            const int y = 435;
            const int height = 27;
            const int quarterOfWidth = 65;

            spriteBatch.Draw(whiteRectangleTexture, new Rectangle(0, y, quarterOfWidth - 1, height), Color.White);
            spriteBatch.Draw(whiteRectangleTexture, new Rectangle(quarterOfWidth, y, quarterOfWidth - 1, height), Color.White);
            spriteBatch.Draw(whiteRectangleTexture, new Rectangle(quarterOfWidth * 2, y, quarterOfWidth - 1, height), Color.White);
            spriteBatch.Draw(whiteRectangleTexture, new Rectangle(quarterOfWidth * 3, y, quarterOfWidth, height), Color.White);

            bool isOnSubmitScoreScreen = activeScreen is SubmitScoreScreen;
            if (isOnSubmitScoreScreen)
            {
                spriteBatch.DrawString(font, "Left", new Vector2(8, y), Color.Black);
                spriteBatch.DrawString(font, "Right", new Vector2(quarterOfWidth + 5, y), Color.Black);
                spriteBatch.DrawString(font, "Down", new Vector2(quarterOfWidth * 2 + 5, y), Color.Black);
                spriteBatch.DrawString(font, "Enter", new Vector2(quarterOfWidth * 3 + 2, y), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(font, "Left", new Vector2(8, y), Color.Black);
                spriteBatch.DrawString(font, "Down", new Vector2(quarterOfWidth + 5, y), Color.Black);
                spriteBatch.DrawString(font, "Flip", new Vector2(quarterOfWidth * 2 + 11, y), Color.Black);
                spriteBatch.DrawString(font, "Right", new Vector2(quarterOfWidth * 3 + 5, y), Color.Black);
            }
        }
    }
}
