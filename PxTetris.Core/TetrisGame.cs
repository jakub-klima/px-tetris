using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace PxTetris.Core
{
    public class TetrisGame : Game
    {
        private readonly Textures textures = new Textures();
        private readonly TopScores topScores = new TopScores();
        private readonly GraphicsDeviceManager graphics;
        protected GameKeyboard keyboard;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        protected IScreen activeScreen;
        protected float scale = 1;

        public TetrisGame()
        {
            graphics = new GraphicsDeviceManager(this);
            keyboard = new GameKeyboard();
            Content.RootDirectory = "Content";
            activeScreen = new InitScreen(topScores);
            Window.Title = "Px Tetris";
        }

        protected override void Initialize()
        {
            AjustGraphics(graphics);
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected virtual void AjustGraphics(GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = 260;
            graphics.PreferredBackBufferHeight = 435;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textures.LoadContent(Content, GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboard.Update();

            if (keyboard.GetPressedKeys().Contains(Keys.Escape))
            {
                Exit();
            }

            activeScreen.Update(keyboard, gameTime.ElapsedGameTime);

            if (activeScreen.NextScreen)
            {
                activeScreen = CreateNextScreen();
                keyboard.Update(); // Fixes keyboard.lastState for Android where keyboard.Update() depends on active screen.
            }

            base.Update(gameTime);
        }

        private IScreen CreateNextScreen()
        {
            switch (activeScreen)
            {
                case InitScreen _:
                    return new GameScreen();

                case GameScreen gameScreen:
                    return new SubmitScoreScreen(topScores, gameScreen.Score);

                case SubmitScoreScreen _:
                    return new InitScreen(topScores);

                default:
                    throw new NotSupportedException($"Not supported screen type {activeScreen.GetType()}");
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: Matrix.CreateScale(scale, scale, 1));
            activeScreen.Draw(spriteBatch, textures, font);
            DrawAdHoc(spriteBatch, textures, font);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected virtual void DrawAdHoc(SpriteBatch spriteBatch, Textures textures, SpriteFont font)
        {
        }
    }
}
