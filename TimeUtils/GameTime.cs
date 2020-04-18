using System;

namespace TimeUtils
{
    public class GameTime
    {
        private DateTime _prevFrame;
        private DateTime _currFrame;

        public long DeltaTimeInMilliseconds { 
            get { return DeltaTimeInTicks != 0 ? DeltaTimeInTicks / TimeSpan.TicksPerMillisecond : 0; } 
            private set { DeltaTimeInTicks = value; } }
        public long DeltaTimeInSeconds { 
            get { return DeltaTimeInTicks != 0 ? DeltaTimeInTicks / TimeSpan.TicksPerSecond : 0; } 
            private set { DeltaTimeInTicks = value; } }
        public long DeltaTimeInTicks { get; private set; }

        public GameTime()
        {
            _prevFrame = DateTime.Now;
            _currFrame = DateTime.Now;
        }

        public void UpdateDeltaTime()
        {
            _currFrame = DateTime.Now;
            DeltaTimeInTicks = (_currFrame.Ticks - _prevFrame.Ticks) /*/ 10000000f*/;
            _prevFrame = _currFrame;
        }
    }
}
