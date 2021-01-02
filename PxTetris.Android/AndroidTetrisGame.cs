using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PxTetris.Core;

namespace PxTetris.Android
{
    public class AndroidTetrisGame : TetrisGame
    {
        int aw;
        int ah;
        int bw;
        int bh;
        int cw;
        int ch;

        protected override void Initialize()
        {
            Rectangle resolution = Window.ClientBounds;

            aw = resolution.Width;
            ah = resolution.Height;
            bw = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            bh = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            cw = GraphicsDevice.Viewport.Width;
            ch = GraphicsDevice.Viewport.Height;

            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            // TODO Toto nechat asi napevno na 1920x1080?
            graphics.PreferredBackBufferWidth = resolution.Width;
            graphics.PreferredBackBufferHeight = resolution.Height;
            graphics.ApplyChanges();

            // TODO Zkusit rozlisit Scale X a Y
            scale = resolution.Width / 260f;
            keyboard = new AndroidGameKeyboard(resolution.Width, () => activeScreen);

            base.Initialize();
        }

        protected override void DrawAdHoc(SpriteBatch spriteBatch, Textures textures, SpriteFont font)
        {
            // TODO [Refactor] Sjednotit kod klaves a jejich souradnic s AndroidGameKeyboard.
            // - Sdilet souradnice sirky atd. s GameScreen, kde jsou zadefinovane (gameAreaOffset atd.)
            // TODO [Refactor] Zarovnat doprostred pomoci font.MeasureString()
            spriteBatch.Draw(textures.WhiteRectangle, new Rectangle(0, 435, 64, 27), Color.White);
            spriteBatch.Draw(textures.WhiteRectangle, new Rectangle(65, 435, 64, 27), Color.White);
            spriteBatch.Draw(textures.WhiteRectangle, new Rectangle(130, 435, 64, 27), Color.White);
            spriteBatch.Draw(textures.WhiteRectangle, new Rectangle(195, 435, 65, 27), Color.White);

            bool isOnSubmitScoreScreen = activeScreen is SubmitScoreScreen;
            spriteBatch.DrawString(font, "Left", new Vector2(8, 435), Color.Black);
            if (isOnSubmitScoreScreen)
            {
                spriteBatch.DrawString(font, "Right", new Vector2(65 + 5, 435), Color.Black);
                spriteBatch.DrawString(font, "Down", new Vector2(130 + 5, 435), Color.Black);
                spriteBatch.DrawString(font, "Enter", new Vector2(195 + 2, 435), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(font, "Down", new Vector2(65 + 5, 435), Color.Black);
                spriteBatch.DrawString(font, "Flip", new Vector2(130 + 11, 435), Color.Black);
                spriteBatch.DrawString(font, "Right", new Vector2(195 + 5, 435), Color.Black);
            }

            //spriteBatch.DrawString(font, a.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, $"{aw}x{ah}", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, $"{bw}x{bh}", new Vector2(10, 30), Color.White);
            spriteBatch.DrawString(font, $"{cw}x{ch}", new Vector2(10, 50), Color.White);

            int dw = Window.ClientBounds.Width;
            int dh = Window.ClientBounds.Height;
            int ew = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int eh = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int fw = GraphicsDevice.Viewport.Width;
            int fh = GraphicsDevice.Viewport.Height;

            spriteBatch.DrawString(font, $"{dw}x{dh}", new Vector2(10, 70), Color.White);
            spriteBatch.DrawString(font, $"{ew}x{eh}", new Vector2(10, 90), Color.White);
            spriteBatch.DrawString(font, $"{fw}x{fh}", new Vector2(10, 110), Color.White);
        }
    }
}
