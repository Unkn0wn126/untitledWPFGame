using Engine.Models.Components;
using Engine.Models.Components.Life;
using Engine.Models.Factories.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Saving
{
    [Serializable]
    public class Save
    {
        public List<byte[]> Scenes { get; set; }
        public int CurrentIndex { get; set; }
    }
}
