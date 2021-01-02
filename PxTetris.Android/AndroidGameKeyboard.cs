using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PxTetris.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PxTetris.Android
{
    public class AndroidGameKeyboard : GameKeyboard
    {
        private readonly int quarterOfWidth;
        private readonly Func<IScreen> activeScreenProvider;

        public AndroidGameKeyboard(int width, Func<IScreen> activeScreenProvider)
        {
            quarterOfWidth = width / 4;
            this.activeScreenProvider = activeScreenProvider;
        }

        protected override KeyboardState GetKeyboardState()
        {
            return new KeyboardState(GetKeys().Distinct().ToArray());
        }

        private IEnumerable<Keys> GetKeys()
        {
            bool isOnSubmitScoreScreen = activeScreenProvider() is SubmitScoreScreen;
            foreach (int x in TouchPanel.GetState().Select(t => t.Position.X))
            {
                if (x < quarterOfWidth)
                {
                    yield return Keys.Left;
                }
                else if (x < quarterOfWidth * 2)
                {
                    yield return isOnSubmitScoreScreen ? Keys.Right : Keys.Down;
                }
                else if (x < quarterOfWidth * 3)
                {
                    yield return isOnSubmitScoreScreen ? Keys.Down : Keys.Up;
                }
                else
                {
                    yield return isOnSubmitScoreScreen ? Keys.Enter : Keys.Right;
                }
            }

            var state = GamePad.GetState(PlayerIndex.One);
            if (state.Buttons.Back == ButtonState.Pressed)
            {
                yield return Keys.P;
            }
        }
    }
}