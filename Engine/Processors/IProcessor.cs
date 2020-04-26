using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Processors
{
    /// <summary>
    /// Interface for logic processing
    /// </summary>
    public interface IProcessor
    {
        void ProcessOneGameTick(float lastFrameTime);
        void ChangeContext(IScene context);
    }
}
