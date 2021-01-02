using Microsoft.Xna.Framework.Graphics;
using System;

namespace PxTetris.Core
{
    public interface IScreen
    {
        bool NextScreen { get; }
        void Update(GameKeyboard keyboard, TimeSpan elapsedTime);
        void Draw(SpriteBatch spriteBatch, Textures textures, SpriteFont font);
    }
}
