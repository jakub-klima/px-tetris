using System;

namespace PxTetris.Core.GameScreenComponents
{
    public class GameTimer
    {
        private TimeSpan length;
        private TimeSpan elapsed;

        public bool TickCompleted => elapsed >= length;

        private void RequestTick(TimeSpan length)
        {
            this.length = length;
            elapsed = TimeSpan.Zero;
        }

        public void Update(TimeSpan elapsedTime)
        {
            elapsed += elapsedTime;
        }
        
        public void RequestLevelTick(Level level)
        {
            RequestTick(level.Interval);
        }

        public void RequestGameOverTick()
        {
            RequestTick(TimeSpan.FromSeconds(3));
        }

        public void RequestGameMessageTick()
        {
            RequestTick(TimeSpan.FromMilliseconds(700));
        }
    }
}
