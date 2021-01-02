using System;

namespace PxTetris.Core.GameScreenComponents
{
    public class Level
    {
        public int Number { get; private set; } = 1;
        public int ScoreToNextLevel { get; private set; } = 500;
        public TimeSpan Interval => TimeSpan.FromMilliseconds(500 * Math.Pow(0.75, Number - 1));

        public void IncreaseLevel()
        {
            Number++;
            ScoreToNextLevel += 500 + (Number - 1) * 100;
        }
    }
}
