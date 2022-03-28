using System;

namespace TimeUtils
{
    /// <summary>
    /// Helper class to keep
    /// track of real time
    /// </summary>
    public class GameTime
    {
        private long _prevFrame;
        private long _currFrame;

        public float DeltaTimeInMilliseconds { get; private set; }
        public float DeltaTimeInSeconds { get; private set; }

        public GameTime()
        {
            _prevFrame = Environment.TickCount;
            _currFrame = Environment.TickCount;
        }

        /// <summary>
        /// Updates the time between
        /// two last ticks
        /// </summary>
        public void UpdateDeltaTime()
        {
            _currFrame = Environment.TickCount;
            DeltaTimeInMilliseconds = (_currFrame - _prevFrame);
            DeltaTimeInSeconds = DeltaTimeInMilliseconds / 1000.0f;
            _prevFrame = _currFrame;
        }
    }
}
