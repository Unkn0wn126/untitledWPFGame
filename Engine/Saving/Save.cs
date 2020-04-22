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
        public List<MetaScene> Scenes { get; set; }
        public MetaScene CurrentScene { get; set; }
        public ITransformComponent PlayerTransform { get; set; }
        public ILifeComponent PlayerLife { get; set; }
    }
}
