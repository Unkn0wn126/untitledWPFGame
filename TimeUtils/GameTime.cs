using System;
using System.Diagnostics;

namespace TimeUtils
{
    public class GameTime
    {
        private long _prevFrame;
        private long _currFrame;

        //private Stopwatch _stopwatch;

        public float DeltaTimeInMilliseconds { get; private set; }
        public float DeltaTimeInSeconds { get; private set; }

        public GameTime()
        {
            _prevFrame = Environment.TickCount;
            _currFrame = Environment.TickCount;
        }

        public void UpdateDeltaTime()
        {
            _currFrame = Environment.TickCount;
            DeltaTimeInMilliseconds = (_currFrame - _prevFrame);
            DeltaTimeInSeconds = DeltaTimeInMilliseconds / 1000.0f;
            _prevFrame = _currFrame;
        }
    }
}
