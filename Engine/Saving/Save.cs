using System;
using System.Collections.Generic;

namespace Engine.Saving
{
    /// <summary>
    /// Mediator class to ease
    /// the saving process
    /// </summary>
    [Serializable]
    public class Save
    {
        public List<byte[]> Scenes { get; set; }
        public int CurrentIndex { get; set; }
    }
}
