using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace PxTetris.Core
{
    public class InitScreen : IScreen
    {
        public bool NextScreen { get; private set; }
        private readonly TopScores topScores;

        public InitScreen(TopScores topScores)
        {
            this.topScores = topScores;
        }

        public void Update(GameKeyboard keyboard, TimeSpan elapsedTime)
        {
            if (keyboard.GetJustPressedKeys().Any())
            {
                NextScreen = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Textures textures, SpriteFont font)
        {
            const int xOffset = 60;
            //spriteBatch.DrawString(font, "Press any key", new Vector2(xOffset, 50), Color.White);
            //spriteBatch.DrawString(font, "to play...", new Vector2(xOffset, 70), Color.White);
            //spriteBatch.DrawString(font, "Top scores:", new Vector2(xOffset, 120), Color.Yellow);

            for (int i = 0; i < topScores.Items.Count; i++)
            {
                spriteBatch.DrawString(font, topScores.Items[i].ToString(), new Vector2(xOffset, 170 + 20 * i), Color.Yellow);
            }
        }
    }
}
