using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PxTetris.Core
{
    public class SubmitScoreScreen : IScreen
    {
        private readonly TopScores topScores;
        private readonly int playedScore;
        public bool NextScreen { get; private set; }
        private int currentLetterIndex = 0;
        private readonly char[] letters = { 'A', 'A', 'A' };

        public SubmitScoreScreen(TopScores topScores, int playedScore)
        {
            this.topScores = topScores;
            this.playedScore = playedScore;
            NextScreen = !this.topScores.IsTopScore(this.playedScore);
        }

        public void Update(GameKeyboard keyboard, TimeSpan elapsedTime)
        {
            IReadOnlyCollection<Keys> justPressedKeys = keyboard.GetJustPressedKeys();
            if (justPressedKeys.Contains(Keys.Enter))
            {
                topScores.InsertTopScore(new string(letters), playedScore);
                NextScreen = true;
            }
            else if (justPressedKeys.Contains(Keys.Up))
            {
                letters[currentLetterIndex]--;
                if (letters[currentLetterIndex] < 'A')
                {
                    letters[currentLetterIndex] = 'Z';
                }
            }
            else if (justPressedKeys.Contains(Keys.Down))
            {
                letters[currentLetterIndex]++;
                if (letters[currentLetterIndex] > 'Z')
                {
                    letters[currentLetterIndex] = 'A';
                }
            }
            else if (justPressedKeys.Contains(Keys.Right))
            {
                currentLetterIndex++;
                if (currentLetterIndex >= letters.Length)
                {
                    currentLetterIndex = 0;
                }
            }
            else if (justPressedKeys.Contains(Keys.Left))
            {
                currentLetterIndex--;
                if (currentLetterIndex < 0)
                {
                    currentLetterIndex = letters.Length - 1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Textures textures, SpriteFont font)
        {
            const int xOffset = 25;
            spriteBatch.DrawString(font, "Congratulations,", new Vector2(xOffset, 40), Color.White);
            spriteBatch.DrawString(font, "you made it to top 5!", new Vector2(xOffset, 60), Color.White);

            spriteBatch.DrawString(font, "Enter player name:", new Vector2(xOffset, 110), Color.White);
            for (int i = 0; i < letters.Length; i++)
            {
                var position = new Vector2(xOffset + i * 15, 130);
                spriteBatch.DrawString(font, letters[i].ToString(), position, Color.Yellow);
                if (currentLetterIndex == i)
                {
                    spriteBatch.DrawString(font, "_", position, Color.Yellow);
                }
            }

            spriteBatch.DrawString(font, "Press enter", new Vector2(xOffset, 180), Color.White);
            spriteBatch.DrawString(font, "to confirm...", new Vector2(xOffset, 200), Color.White);
        }
    }
}
