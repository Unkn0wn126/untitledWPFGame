using Engine.Models.Components;
using Engine.Models.Factories.Entities;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Factories.Scenes
{
    public class MetaMapEntity
    {
        public ComponentState Components { get; set; }
        public ImgName Graphics { get; set; }
        public CollisionType CollisionType { get; set; }
        public int ZIndex { get; set; }
    }
}
