using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PxTetris.Core.GameScreenComponents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PxTetris.Core
{
    public class GameScreen : IScreen
    {
        private const int gameAreaOffset = 75;

        public bool NextScreen { get; private set; }
        public int Score { get; private set; }

        private readonly Level level = new Level();
        private readonly GameTimer timer = new GameTimer();
        private readonly SquareGrid squares = new SquareGrid();
        private Brick activeBrick = new Brick();
        private Brick nextBrick = new Brick();
        private bool paused;
        private GameState state = GameState.Running;
        private int rowsClearedCombo;
        private DateTime lastMoveTime;

        public GameScreen()
        {
            timer.RequestLevelTick(level);
        }

        public void Update(GameKeyboard keyboard, TimeSpan elapsedTime)
        {
            IReadOnlyCollection<Keys> pressedKeys = keyboard.GetPressedKeys();
            IReadOnlyCollection<Keys> justPressedKeys = keyboard.GetJustPressedKeys();
            if (justPressedKeys.Contains(Keys.P))
            {
                paused = !paused;
            }
            if (paused) return;

            timer.Update(elapsedTime);

            switch (state)
            {
                case GameState.Running:
                    UpdateRunning(pressedKeys, justPressedKeys);
                    break;

                case GameState.RowClearing:
                    UpdateRowClearing();
                    break;

                case GameState.LevelUp:
                    if (timer.TickCompleted)
                    {
                        timer.RequestGameMessageTick();
                        state = GameState.RowClearing;
                    }
                    break;

                case GameState.GameOver:
                    if (timer.TickCompleted)
                    {
                        NextScreen = true;
                    }
                    break;
            }
        }

        private void UpdateRunning(IReadOnlyCollection<Keys> pressedKeys, IReadOnlyCollection<Keys> justPressedKeys)
        {
            if (justPressedKeys.Contains(Keys.Up) || justPressedKeys.Contains(Keys.Space))
            {
                activeBrick.Rotate();
                if (!ActiveBrickPositionValid())
                {
                    activeBrick.UndoRotation();
                }
            }

            if (pressedKeys.Contains(Keys.Right) && LastMoveCooldownElapsed())
            {
                lastMoveTime = DateTime.Now;
                activeBrick.MoveRight();
                if (!ActiveBrickPositionValid())
                {
                    activeBrick.UndoMove();
                }
            }
            else if (pressedKeys.Contains(Keys.Left) && LastMoveCooldownElapsed())
            {
                lastMoveTime = DateTime.Now;
                activeBrick.MoveLeft();
                if (!ActiveBrickPositionValid())
                {
                    activeBrick.UndoMove();
                }
            }

            if (timer.TickCompleted ||
                (pressedKeys.Contains(Keys.Down) && LastMoveCooldownElapsed()))
            {
                lastMoveTime = DateTime.Now;
                OnGameTick();
            }
        }

        private bool LastMoveCooldownElapsed()
        {
            return lastMoveTime.Add(TimeSpan.FromMilliseconds(80)) < DateTime.Now;
        }

        private void OnGameTick()
        {
            activeBrick.MoveDown();
            timer.RequestLevelTick(level);
            if (!ActiveBrickPositionValid())
            {
                activeBrick.UndoMove();
                DissolveActiveBrick();
                if (squares.AnyRowFull())
                {
                    state = GameState.RowClearing;
                    timer.RequestGameMessageTick();
                }
                else
                {
                    PrepareNewBrick();
                }
            }
        }

        private void PrepareNewBrick()
        {
            activeBrick = nextBrick;
            nextBrick = new Brick();

            if (!ActiveBrickPositionValid())
            {
                state = GameState.GameOver;
                timer.RequestGameOverTick();
            }
        }

        private void DissolveActiveBrick()
        {
            squares.AddBrick(activeBrick);
            activeBrick = null;
        }

        private bool ActiveBrickPositionValid()
        {
            if (activeBrick.Position.X < 0 ||
                activeBrick.Position.Y < 0 ||
                activeBrick.Position.X + activeBrick.Squares.GetLength(0) > squares.Items.GetLength(0) ||
                activeBrick.Position.Y + activeBrick.Squares.GetLength(1) > squares.Items.GetLength(1))
            {
                return false;
            }

            for (int x = 0; x < activeBrick.Squares.GetLength(0); x++)
            {
                for (int y = 0; y < activeBrick.Squares.GetLength(1); y++)
                {
                    if (activeBrick.Squares[x, y] != null &&
                        squares.Items[activeBrick.Position.X + x, activeBrick.Position.Y + y] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void UpdateRowClearing()
        {
            if (!timer.TickCompleted) return;

            if (squares.AnyRowFull())
            {
                squares.ClearRow();
                rowsClearedCombo++;
                Score += 100 * (int)Math.Pow(2, rowsClearedCombo - 1);
                if (Score > level.ScoreToNextLevel)
                {
                    level.IncreaseLevel();
                    state = GameState.LevelUp;
                }
                timer.RequestGameMessageTick();
            }
            else
            {
                rowsClearedCombo = 0;
                state = GameState.Running;
                timer.RequestLevelTick(level);
                PrepareNewBrick();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Textures textures, SpriteFont font)
        {
            DrawInfoPanel(spriteBatch, textures, font);
            squares.Draw(spriteBatch, textures, gameAreaOffset);
            if (activeBrick != null)
            {
                DrawActiveBrick(spriteBatch, textures);
            }

            if (state == GameState.GameOver)
            {
                spriteBatch.DrawString(font, "GAME OVER", new Vector2(80, 140), Color.White);
            }
            else if (state == GameState.LevelUp)
            {
                spriteBatch.DrawString(font, $"LEVEL {level.Number}!", new Vector2(80, 140), Color.White);
            }
            else if (rowsClearedCombo > 0)
            {
                spriteBatch.DrawString(font, $"COMBO x{rowsClearedCombo}", new Vector2(80, 140), Color.White);
            }

            if (paused)
            {
                spriteBatch.DrawString(font, "PAUSE", new Vector2(100, 200), Color.White);
            }
        }

        private void DrawInfoPanel(SpriteBatch spriteBatch, Textures textures, SpriteFont font)
        {
            const int infoPanelHeight = gameAreaOffset;

            spriteBatch.Draw(textures.WhiteRectangle, new Rectangle(0, 0, squares.Items.GetLength(0) * Square.Size, infoPanelHeight), Color.White);
            spriteBatch.DrawString(font, $"Level: {level.Number}", new Vector2(10, 5), Color.Black);
            spriteBatch.DrawString(font, $"Score: {Score}", new Vector2(10, 25), Color.Black);
            spriteBatch.DrawString(font, $"Next level: {level.ScoreToNextLevel}", new Vector2(10, 45), Color.Black);

            spriteBatch.DrawString(font, "Next:", new Vector2(200, 5), Color.Black);
            DrawNextBrick(spriteBatch, textures);
        }

        private void DrawNextBrick(SpriteBatch spriteBatch, Textures textures)
        {
            for (int x = 0; x < nextBrick.Squares.GetLength(0); x++)
            {
                for (int y = 0; y < nextBrick.Squares.GetLength(1); y++)
                {
                    nextBrick.Squares[x, y]?.Draw(spriteBatch, textures, x, y, 200, 30, 0.5f);
                }
            }
        }

        private void DrawActiveBrick(SpriteBatch spriteBatch, Textures textures)
        {
            for (int x = 0; x < activeBrick.Squares.GetLength(0); x++)
            {
                for (int y = 0; y < activeBrick.Squares.GetLength(1); y++)
                {
                    activeBrick.Squares[x, y]?.Draw(spriteBatch, textures, activeBrick.Position.X + x, activeBrick.Position.Y + y, 0, gameAreaOffset);
                }
            }
        }
    }
}
