namespace PxTetris.Core
{
    public class TopScore
    {
        public string Player { get; }
        public int Score { get; }

        public TopScore(string player, int score)
        {
            Player = player;
            Score = score;
        }

        public TopScore(string sourceLine)
            : this(
                  player: sourceLine.Substring(0, 3),
                  score: int.Parse(sourceLine.Substring(4)))
        {
        }

        public override string ToString()
        {
            return $"{Player}: {Score}";
        }
    }
}
