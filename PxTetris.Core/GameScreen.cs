using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PxTetris.Core.GameScreenComponents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PxTetris.Core
{
    // TODO [Refactor] Zavest pomocne metody / tridy
    public class GameScreen : IScreen
    {
        private const int squareSize = 20;
        const int gameAreaOffset = 75;

        public bool NextScreen { get; private set; }
        public int Score { get; private set; }

        private readonly Level level = new Level();
        private readonly GameTimer timer = new GameTimer();
        private readonly Square?[,] squares = new Square?[13, 18];
        private Brick brick = new Brick();
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
            if (paused)
            {
                return;
            }

            timer.Update(elapsedTime);

            switch (state)
            {
                case GameState.Running:
                    if (justPressedKeys.Contains(Keys.Up) || justPressedKeys.Contains(Keys.Space))
                    {
                        brick.Rotate();
                        if (!BrickPositionValid())
                        {
                            brick.UndoRotation();
                        }
                    }

                    if (pressedKeys.Contains(Keys.Right) && LastMoveCooldownElapsed())
                    {
                        lastMoveTime = DateTime.Now;
                        brick.MoveRight();
                        if (!BrickPositionValid())
                        {
                            brick.UndoMove();
                        }
                    }
                    else if (pressedKeys.Contains(Keys.Left) && LastMoveCooldownElapsed())
                    {
                        lastMoveTime = DateTime.Now;
                        brick.MoveLeft();
                        if (!BrickPositionValid())
                        {
                            brick.UndoMove();
                        }
                    }

                    if (timer.TickCompleted ||
                        (pressedKeys.Contains(Keys.Down) && LastMoveCooldownElapsed()))
                    {
                        lastMoveTime = DateTime.Now;
                        OnGameTick();
                    }
                    break;

                case GameState.RowClearing:
                    if (timer.TickCompleted)
                    {
                        if (AnyRowFull())
                        {
                            ClearRow();
                            rowsClearedCombo++;
                            Score += 100 * (int)Math.Pow(2, rowsClearedCombo - 1);
                            if (level.HasScoreForNextLevel(Score))
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

        private bool LastMoveCooldownElapsed()
        {
            return lastMoveTime.Add(TimeSpan.FromMilliseconds(80)) < DateTime.Now;
        }

        private void OnGameTick()
        {
            brick.MoveDown();
            timer.RequestLevelTick(level);
            if (!BrickPositionValid())
            {
                brick.UndoMove();
                DissolveBrick();
                if (AnyRowFull())
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
            brick = nextBrick;
            nextBrick = new Brick();

            if (!BrickPositionValid())
            {
                state = GameState.GameOver;
                timer.RequestGameOverTick();
            }
        }

        private void DissolveBrick()
        {
            for (int x = 0; x < brick.Squares.GetLength(0); x++)
            {
                for (int y = 0; y < brick.Squares.GetLength(1); y++)
                {
                    if (brick.Squares[x, y].HasValue)
                    {
                        squares[brick.Position.X + x, brick.Position.Y + y] = brick.Squares[x, y];
                    }
                }
            }

            brick = null;
        }

        private bool AnyRowFull()
        {
            return GetFullRow().HasValue;
        }

        private void ClearRow()
        {
            int fullRow = GetFullRow().Value;
            for (int y = fullRow - 1; y >= 0; y--)
            {
                for (int x = 0; x < squares.GetLength(0); x++)
                {
                    squares[x, y + 1] = squares[x, y];
                }
            }
        }

        private int? GetFullRow()
        {
            for (int y = squares.GetLength(1) - 1; y >= 0; y--)
            {
                bool isRowFull = true;

                for (int x = 0; x < squares.GetLength(0); x++)
                {
                    if (!squares[x, y].HasValue)
                    {
                        isRowFull = false;
                        break;
                    }
                }

                if (isRowFull)
                {
                    return y;
                }
            }

            return null;
        }

        private bool BrickPositionValid()
        {
            if (brick.Position.X < 0 ||
                brick.Position.Y < 0 ||
                brick.Position.X + brick.Squares.GetLength(0) > squares.GetLength(0) ||
                brick.Position.Y + brick.Squares.GetLength(1) > squares.GetLength(1))
            {
                return false;
            }

            for (int x = 0; x < brick.Squares.GetLength(0); x++)
            {
                for (int y = 0; y < brick.Squares.GetLength(1); y++)
                {
                    if (brick.Squares[x, y].HasValue &&
                        squares[brick.Position.X + x, brick.Position.Y + y].HasValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void Draw(SpriteBatch spriteBatch, Textures textures, SpriteFont font)
        {
            spriteBatch.Draw(textures.WhiteRectangle, new Rectangle(0, 0, squares.GetLength(0) * squareSize, gameAreaOffset), Color.White);
            spriteBatch.DrawString(font, $"Level: {level.Number}", new Vector2(10, 5), Color.Black);
            spriteBatch.DrawString(font, $"Score: {Score}", new Vector2(10, 25), Color.Black);
            spriteBatch.DrawString(font, $"Next level: {level.ScoreToNextLevel}", new Vector2(10, 45), Color.Black);

            spriteBatch.DrawString(font, "Next:", new Vector2(200, 5), Color.Black);
            for (int x = 0; x < nextBrick.Squares.GetLength(0); x++)
            {
                for (int y = 0; y < nextBrick.Squares.GetLength(1); y++)
                {
                    if (nextBrick.Squares[x, y].HasValue)
                    {
                        DrawSquare(spriteBatch, textures, nextBrick.Squares[x, y].Value, x, y, 200, 30, 0.5f);
                    }
                }
            }

            if (brick != null)
            {
                for (int x = 0; x < brick.Squares.GetLength(0); x++)
                {
                    for (int y = 0; y < brick.Squares.GetLength(1); y++)
                    {
                        if (brick.Squares[x, y].HasValue)
                        {
                            DrawSquare(spriteBatch, textures, brick.Squares[x, y].Value, brick.Position.X + x, brick.Position.Y + y);
                        }
                    }
                }
            }

            for (int x = 0; x < squares.GetLength(0); x++)
            {
                for (int y = 0; y < squares.GetLength(1); y++)
                {
                    if (squares[x, y].HasValue)
                    {
                        DrawSquare(spriteBatch, textures, squares[x, y].Value, x, y);
                    }
                }
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

        private void DrawSquare(SpriteBatch spriteBatch, Textures textures, Square square, int x, int y, int xOffset = 0, int yOffset = gameAreaOffset, float scale = 1)
        {
            Texture2D texture = GetTexture(textures, square);
            Rectangle destination = new Rectangle(xOffset + (int)(x * squareSize * scale), yOffset + (int)(y * squareSize * scale), (int)(squareSize * scale), (int)(squareSize * scale));
            spriteBatch.Draw(texture, destination, Color.White);
        }

        private Texture2D GetTexture(Textures textures, Square square)
        {
            switch (square)
            {
                case Square.Blue:
                    return textures.BrickBlue;
                case Square.Green:
                    return textures.BrickGreen;
                case Square.Red:
                    return textures.BrickRed;
                case Square.Yellow:
                    return textures.BrickYellow;
                case Square.Purple:
                    return textures.BrickPurple;
                default:
                    throw new ArgumentOutOfRangeException(nameof(square));
            }
        }
    }
}
