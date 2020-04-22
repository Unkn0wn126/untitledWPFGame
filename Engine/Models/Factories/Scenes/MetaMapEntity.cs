using Engine.Models.Components;
using Engine.Models.Components.Script;
using Engine.Models.Factories.Entities;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Factories.Scenes
{
    [Serializable]
    public class MetaMapEntity
    {
        public ComponentState Components { get; set; }
        public ImgName Graphics { get; set; }
        public CollisionType CollisionType { get; set; }
        public ScriptType Scripts { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float SizeX { get; set; }
        public float SizeY { get; set; }
        public int ZIndex { get; set; }
    }
}
