using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace PxTetris.Core
{
    public class GameKeyboard
    {
        private KeyboardState lastState;
        private KeyboardState state = new KeyboardState();

        public void Update()
        {
            lastState = state;
            state = GetKeyboardState();
        }

        protected virtual KeyboardState GetKeyboardState()
        {
            return Keyboard.GetState();
        }

        public IReadOnlyCollection<Keys> GetJustPressedKeys()
        {
            return state.GetPressedKeys()
                .Except(lastState.GetPressedKeys())
                .ToArray();
        }

        public IReadOnlyCollection<Keys> GetPressedKeys()
        {
            return state.GetPressedKeys();
        }
    }
}
