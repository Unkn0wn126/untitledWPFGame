using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Processors
{
    /// <summary>
    /// This should take care of the logic...
    /// </summary>
    public interface IProcessor
    {
        void ProcessOneGameTick(long lastFrameTime);
    }
}
